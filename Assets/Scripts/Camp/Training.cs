using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Training : MonoBehaviour {

    public MonsterCollection monsterCollection;

    public GameObject trainingButton; // The Button to click when the training is finished
    public Sprite[] rewardImages;

    public GameObject monsterManagePanel;
    public GameObject monsterButtonPrefab;
    public GameObject monsterList;

    // When a players clicks on a monster, this screen appears
    public GameObject monsterView;
    public Text monsterNameTitle;
    public Image monsterImage;
    public Text[] numberOfItems;
    public Button[] trainingChoiceButtons;

    // When the player tries to train a monster when one is already training
    public GameObject trainingNowPanel;

    //Appears when the training is finished
    public GameObject traningEndPanel;
    public Text trainingEndText;

    private string monsterInTraining; // The name of the monster that is training;

    private string tempChosenMonster;

    public bool trainingNow = false;
    private int attributeToTrain;
    private int attributeIncrease; // the amount to increase the attribute

    [SerializeField] private int minIncreaseValue = 8;
    [SerializeField] private int maxIncreaseValue = 16;

    private void Start()
    {
        if (PlayerPrefs.HasKey("_training"))
        {
            if (PlayerPrefs.GetInt("_training") == 1)
            {
                trainingNow = true;
                trainingButton.SetActive(true);
            }
        }
    }

    public void ShowTrainingNowPanel()
    {
        trainingNowPanel.SetActive(true);
    }

    public void HideTrainingNowPanel()
    {
        trainingNowPanel.SetActive(false);
    }

    public void HideEndTrainingPanel()
    {
        traningEndPanel.SetActive(false);
    }

    public void ShowTrainingChoice(Sprite image, string monster)
    {

        string monsterTrainTitle = LocalizationManager.sharedInstance.localizedText["train"] + " " + monster;

        monsterNameTitle.text = monsterTrainTitle;
        monsterImage.sprite = image;

        tempChosenMonster = monster;

        // This sets the three items to the correct numbers
        numberOfItems[0].text = Inventory.sharedInstance.inventoryItems[0].ToString(); 
        numberOfItems[1].text = Inventory.sharedInstance.inventoryItems[1].ToString();
        numberOfItems[2].text = Inventory.sharedInstance.inventoryItems[2].ToString();

        for (int i = 0; i < Inventory.sharedInstance.inventoryItems.Length; i++)
        {
            if (Inventory.sharedInstance.inventoryItems[i] == 0)
            {
                trainingChoiceButtons[i].interactable = false;
            }
        }

        ShowHideMonsterView();
    }

    public void ShowHideMonsterView()
    {
        monsterView.SetActive(!monsterView.activeSelf);
    }

    public void Train(int attribute)
    {
        
        trainingNow = true;

        Inventory.sharedInstance.inventoryItems[attribute]--;
        Inventory.sharedInstance.SaveInventory();
        monsterInTraining = tempChosenMonster;
        MonsterCollector.sharedInstance.PutInUnavailableList(monsterInTraining);

        trainingButton.transform.GetChild(1).GetComponent<Image>().sprite = rewardImages[attribute];
        trainingButton.SetActive(true);
        trainingButton.GetComponent<TrainingRewardTimer>().StartTimer();

        attributeToTrain = attribute;

        int value = Random.Range(minIncreaseValue, maxIncreaseValue);

        attributeIncrease = value;

        PlayerPrefs.SetInt("_training", 1);
        PlayerPrefs.SetInt("_trainingCategory", attributeToTrain);
        PlayerPrefs.SetInt("_attributeIncrease", attributeIncrease);
        PlayerPrefs.SetString("_monsterInTraining", monsterInTraining);

        monsterCollection.UpdateSpawnedMonsters();

        monsterManagePanel.SetActive(false);
        monsterView.SetActive(false);

    }

    public void EndTraining()
    {

        monsterInTraining = PlayerPrefs.GetString("_monsterInTraining");
        attributeToTrain = PlayerPrefs.GetInt("_trainingCategory");
        attributeIncrease = PlayerPrefs.GetInt("_attributeIncrease");

        if (attributeToTrain == 0) //Strength was trained
        {
            MonsterCollector.sharedInstance.unlockedMonsters[monsterInTraining].Strength += attributeIncrease;
            if (LocalizationManager.sharedInstance.isFrench)
            {
                trainingEndText.text = "La force de " + monsterInTraining + " a augmenté de " + attributeIncrease + "!";
            }
            else
            {
                trainingEndText.text = monsterInTraining + "'s strength increased by " + attributeIncrease + "!";
            }
            MonsterCollector.sharedInstance.SaveList();
        }
        else if (attributeToTrain == 1) //Intelligence was trained
        {
            MonsterCollector.sharedInstance.unlockedMonsters[monsterInTraining].Intelligence += attributeIncrease;
            if (LocalizationManager.sharedInstance.isFrench)
            {
                trainingEndText.text = "L'intelligence de " + monsterInTraining + " a augmenté de " + attributeIncrease + "!";
            }
            else
            {
                trainingEndText.text = monsterInTraining + "'s intelligence increased by " + attributeIncrease + "!";
            }
            
            MonsterCollector.sharedInstance.SaveList();
        }
        else if (attributeToTrain == 2) //Life was trained
        {
            MonsterCollector.sharedInstance.unlockedMonsters[monsterInTraining].Life += attributeIncrease;
            if (LocalizationManager.sharedInstance.isFrench)
            {
                trainingEndText.text = "La vie de " + monsterInTraining + " a augmenté de " + attributeIncrease + "!";
            }
            else
            {
                trainingEndText.text = monsterInTraining + "'s life increased by " + attributeIncrease + "!";
            }
            MonsterCollector.sharedInstance.SaveList();
        }

        MonsterCollector.sharedInstance.RemoveFromUnavailableList(monsterInTraining);
        trainingNow = false;
        PlayerPrefs.SetInt("_training", 0);

        monsterCollection.UpdateSpawnedMonsters();

        traningEndPanel.SetActive(true);
    }
}
