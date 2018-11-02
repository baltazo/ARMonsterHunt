using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManageScreen : MonoBehaviour {

    public Training training;
    public Breeding breeding;
    public Fighting fighting;

    public GameObject monsterManagePanel;
    public GameObject monsterList;
    public Text screenTitle;

    public GameObject monsterButtonPrefab;

    private bool monsterListPopulated = false;
    private string activeScreen;

    private void Start()
    {
        GameController.sharedInstance.monsterManage = this;
    }

    public void ShowSelectedList(string screenSelected)
    {

        activeScreen = screenSelected;

        Debug.Log(activeScreen);

        if (!monsterListPopulated)
        {
            foreach (Monster monster in MonsterCollector.sharedInstance.unlockedMonsters.Values)
            {
                foreach (Sprite image in MonsterCollector.sharedInstance.monstersImages)
                {
                    if (monster.PrefabName == image.name)
                    {
                        monsterButtonPrefab.transform.GetChild(0).GetComponent<Image>().sprite = image;
                        monsterButtonPrefab.transform.GetChild(1).GetComponent<Text>().text = monster.Name;
                        monsterButtonPrefab.GetComponent<MonsterViewButton>().monsterManage = this;

                        Instantiate(monsterButtonPrefab, monsterList.transform);
                    }
                }
            }

            float numberOfMonsters = MonsterCollector.sharedInstance.unlockedMonsters.Count + 0.2f;
            float heightOfList = Mathf.Round(numberOfMonsters / 2) * 600f;

            RectTransform rt = monsterList.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, heightOfList);

            monsterListPopulated = true;
        }

        for (int i = 0; i < monsterList.transform.childCount; i++)
        {
            Transform button = monsterList.transform.GetChild(i);
            string monsterName = monsterList.transform.GetChild(i).GetChild(1).GetComponent<Text>().text;
            if (MonsterCollector.sharedInstance.unavailableMonsters.Contains(monsterName))
            {
                button.GetComponent<Button>().interactable = false;
                button.GetChild(2).gameObject.SetActive(true);
                button.GetChild(2).GetComponent<Text>().text = LocalizationManager.sharedInstance.localizedText["unavailable"];
            }
            else
            {
                button.GetComponent<Button>().interactable = true;
                button.transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        if (activeScreen == "Breeding")
        {
            if (breeding.breedingNow)
            {
                breeding.ShowBreedingNowPanel();
                return;
            }
            else if (MonsterCollector.sharedInstance.unlockedMonsters.Count < 2)
            {
                breeding.ShowHideCannotBreedPanel();
                return;
            }
            else
            {
                screenTitle.text = LocalizationManager.sharedInstance.localizedText["fusion_choice"];
                monsterManagePanel.SetActive(true);
            }
        }

        if (activeScreen == "Training")
        {
            if (training.trainingNow)
            {
                training.ShowTrainingNowPanel();
                return;
            }
            else
            {
                screenTitle.text = LocalizationManager.sharedInstance.localizedText["training"];
                monsterManagePanel.SetActive(true);
            }
        }

        if (activeScreen == "Fighting")
        {
            screenTitle.text = LocalizationManager.sharedInstance.localizedText["fighting"];
            monsterManagePanel.SetActive(true);
        }
        
    }

    public void MonsterAction(Sprite image, string monster)
    {
        if (activeScreen == "Training")
        {
            training.ShowTrainingChoice(image, monster);
        }
        else if (activeScreen == "Breeding")
        {
            breeding.ChooseMonstersToBreed(image, monster);
        }
        else if (activeScreen == "Fighting")
        {
            fighting.ChooseMonsterToFight(monster);
        }

        
    }

    public void HideManagePanel()
    {
        monsterManagePanel.SetActive(false);

        if (activeScreen == "Breeding")
        {
            breeding.CancelBreeding();
        }
    }

    public void ResetList()
    {

        for (int i = 0; i < monsterList.transform.childCount; i++)
        {
            monsterList.transform.GetChild(i).gameObject.SetActive(false);
        }

        monsterListPopulated = false;
    }
}
