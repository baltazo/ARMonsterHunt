using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breeding : MonoBehaviour {

    public MonsterCollection monsterCollection;
    public MonsterManageScreen monsterManageScript;

    public GameObject breedingButton;

    public GameObject monsterManageScreen;
    public Transform monsterList;

    public GameObject breedMonsterPanel;
    public Image breedMonster1;
    public Image breedMonster2;
    public Text nameMonster1;
    public Text nameMonster2;


    public GameObject fusedMonsterPanel; // The same kind of panel as the color hunt scene
    public Image fusedMonsterImage;
    public Text fusedMonsterRace;
    public Text fusedMonsterName;
    public GameObject inputField;

    private List<Monster> monstersToBreed = new List<Monster>();
    private string monsterName1;
    private string monsterName2;

    private Sprite[] monsterImages = new Sprite[2];

    public bool breedingNow = false;

    private GameObject newMonster;

    public GameObject breedingNowPanel;
    public GameObject cannotBreedPanel;

    private void Start()
    {
        if (PlayerPrefs.HasKey("_breeding"))
        {
            if (PlayerPrefs.GetInt("_breeding") == 1)
            {
                breedingNow = true;
                breedingButton.SetActive(true);
            }
        }
    }

    public void ChooseMonstersToBreed(Sprite image, string monster)
    {
        Monster monsterToAdd = MonsterCollector.sharedInstance.unlockedMonsters[monster];
        monstersToBreed.Add(monsterToAdd);
        MonsterCollector.sharedInstance.unavailableMonsters.Add(monster);

        if(monstersToBreed.Count == 1)
        {
            monsterImages[0] = image;
            monsterName1 = monster;
        }

        for (int i = 0; i < monsterList.childCount; i++)
        {
            Transform button = monsterList.GetChild(i);
            if (button.GetChild(1).GetComponent<Text>().text == monster)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }

        if (monstersToBreed.Count == 2)
        {
            monsterImages[1] = image;
            monsterName2 = monster;
            ConfirmBreeding();
        }

    }

    private void ConfirmBreeding()
    {
        breedMonster1.sprite = monsterImages[0];
        breedMonster2.sprite = monsterImages[1];
        nameMonster1.text = monsterName1;
        nameMonster2.text = monsterName2;

        breedMonsterPanel.SetActive(true);
        
    }

    public void StartBreeding()
    {
        PlayerPrefs.SetInt("_breeding", 1);
        breedingNow = true;
        foreach (Monster monster in monstersToBreed)
        {
            MonsterCollector.sharedInstance.PutInUnavailableList(monster.Name);
        }
        breedingButton.SetActive(true);
        breedingButton.GetComponent<BreedingRewardTimer>().StartTimer();

        PlayerPrefs.SetString("_monsterFusing1", monsterName1);
        PlayerPrefs.SetString("_monsterFusing2", monsterName2);

        monsterCollection.UpdateSpawnedMonsters();

        monsterManageScreen.SetActive(false);
        breedMonsterPanel.SetActive(false);
    }

    public void GetNewMonster()
    {
        float r = Random.Range(0, 1f);
        float g = Random.Range(0, 1f);
        float b = Random.Range(0, 1f);

        Color newMonsterColor = new Color(r, g, b);

        GameObject fusedMonster = MonsterCollector.sharedInstance.MonsterChooser(newMonsterColor);
        newMonster = fusedMonster;

        fusedMonsterRace.text = fusedMonster.name + "!";

        foreach (Sprite image in MonsterCollector.sharedInstance.monstersImages)
        {
            if (fusedMonster.name == image.name)
            {
                fusedMonsterImage.sprite = image;
                break;
            }
        }

        monsterCollection.UpdateSpawnedMonsters();

        fusedMonsterPanel.SetActive(true);
        inputField.SetActive(true);
}

    public void CancelBreeding()
    {
        foreach (Monster monster in monstersToBreed)
        {
            MonsterCollector.sharedInstance.RemoveFromUnavailableList(monster.Name);
        }
        monstersToBreed.Clear();

        for (int i = 0; i < monsterList.childCount; i++)
        {
            Transform button = monsterList.GetChild(i);
            button.GetComponent<Button>().interactable = true;
        }

        breedMonsterPanel.SetActive(false);
    }

    public void SetNameAndAddToList(string name)
    {
        fusedMonsterName.text = name;

        string monster1Name = PlayerPrefs.GetString("_monsterFusing1");
        string monster2Name = PlayerPrefs.GetString("_monsterFusing2");

        Monster monster1 = MonsterCollector.sharedInstance.unlockedMonsters[monster1Name];
        Monster monster2 = MonsterCollector.sharedInstance.unlockedMonsters[monster2Name];

        int strength = Mathf.RoundToInt((monster1.Strength + monster2.Strength) / 2);
        int intel = Mathf.RoundToInt((monster1.Intelligence + monster2.Intelligence) / 2);
        int life = Mathf.RoundToInt((monster1.Life + monster2.Life) / 2);

        MonsterCollector.sharedInstance.AddFusedMonsterToList(newMonster, name, strength, intel, life);

        MonsterCollector.sharedInstance.RemoveFromUnavailableList(monster1Name);
        MonsterCollector.sharedInstance.RemoveFromUnavailableList(monster2Name);

        MonsterCollector.sharedInstance.RemoveFromList(monster1Name);
        MonsterCollector.sharedInstance.RemoveFromList(monster2Name);

        monsterCollection.UpdateSpawnedMonsters();

        breedingButton.SetActive(false);
        inputField.SetActive(false);
        PlayerPrefs.SetInt("_breeding", 0);
        CancelBreeding();
        breedingNow = false;
    }

    public void CloseFusedPanel()
    {
        fusedMonsterPanel.SetActive(false);
        monsterCollection.ResetList();
        monsterManageScript.ResetList();
    }

    public void ShowBreedingNowPanel()
    {
        breedingNowPanel.SetActive(true);
    }

    public void CloseBreedingNowPanel()
    {
        breedingNowPanel.SetActive(false);
    }

    public void ShowHideCannotBreedPanel()
    {
        cannotBreedPanel.SetActive(!cannotBreedPanel.activeSelf);
    }

}
