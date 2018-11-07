using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCollection : MonoBehaviour {

    public Transform[] spawnPoints;

    // References to the Monster Collection List
    public GameObject collectionPanel;
    public GameObject monsterButtonPrefab;
    public GameObject[] monsterList;

    // When a players clicks on a monster, this screen appears
    public GameObject monsterView;
    public Image monsterImage;
    public Transform monsterInfo;

    public Button previousPageButton;
    public Button nextPageButton;

    public Image[] pagination;
    public Sprite activePage;
    public Sprite inactivePage;

    private int spawnedMonster = 0;
    private bool monsterCollectionPopulated = false;
    private int activeCollectionPage = 0;
    private int maxPage = 5; // Change this depending on the max number of monster you want

	// Use this for initialization
	void Start () {

        UpdateSpawnedMonsters();
        monsterList[0].SetActive(true);
        if (MonsterCollector.sharedInstance.unavailableMonsters.Count > 8)
        {
            nextPageButton.interactable = true;
        }
    }

    public void UpdateSpawnedMonsters()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Monster")
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        spawnedMonster = 0;

        foreach (Monster monster in MonsterCollector.sharedInstance.unlockedMonsters.Values)
        {

            if (!MonsterCollector.sharedInstance.unavailableMonsters.Contains(monster.Name))
            {
                GameObject monsterToInstantiate = MonsterCollector.sharedInstance.monsterPrefabsList[monster.PrefabName];

                monsterToInstantiate.GetComponent<MonsterAttributes>().SetAttributes(monster);

                Instantiate(monsterToInstantiate, spawnPoints[spawnedMonster].position, Quaternion.identity, spawnPoints[spawnedMonster].transform);

                spawnedMonster++;
                if (spawnedMonster == 4)
                {
                    break;
                }
            }

        }
    }

    public void ShowCollection()
    {
        if (!monsterCollectionPopulated)
        {

            int buttonsAdded = 0;
            int monsterCollectionPage = 0;

            foreach (Monster monster in MonsterCollector.sharedInstance.unlockedMonsters.Values)
            {
                foreach (Sprite image in MonsterCollector.sharedInstance.monstersImages)
                {
                    if (monster.PrefabName == image.name)
                    {
                        monsterButtonPrefab.transform.GetChild(0).GetComponent<Image>().sprite = image;
                        monsterButtonPrefab.transform.GetChild(1).GetComponent<Text>().text = monster.Name;
                        monsterButtonPrefab.GetComponent<MonsterViewButton>().monsterCollection = this;
                        Instantiate(monsterButtonPrefab, monsterList[monsterCollectionPage].transform);
                        buttonsAdded++;
                    }
                }
                if (buttonsAdded == 8)
                {
                    monsterCollectionPage++;
                    buttonsAdded = 0;
                    nextPageButton.interactable = true;
                }
            }

            monsterCollectionPopulated = true;
        }

        collectionPanel.SetActive(true);
        
    }

    public void NextPage() // TODO: Maybe use an animation to switch pages
    {
        monsterList[activeCollectionPage].SetActive(false);
        pagination[activeCollectionPage].sprite = inactivePage;
        activeCollectionPage++;
        monsterList[activeCollectionPage].SetActive(true);
        pagination[activeCollectionPage].sprite = activePage;
        if (activeCollectionPage == maxPage)
        {
            nextPageButton.interactable = false;
        }
        else if (monsterList[activeCollectionPage+1].transform.childCount == 0)
        {
            nextPageButton.interactable = false;
        }
        else
        {
            nextPageButton.interactable = true;
        }

        previousPageButton.interactable = true;
    }

    public void PreviousPage()
    {
        monsterList[activeCollectionPage].SetActive(false);
        pagination[activeCollectionPage].sprite = inactivePage;
        activeCollectionPage--;
        monsterList[activeCollectionPage].SetActive(true);
        pagination[activeCollectionPage].sprite = activePage;

        if (activeCollectionPage == 0)
        {
            previousPageButton.interactable = false;
        }
        else
        {
            previousPageButton.interactable = true;
        }

        nextPageButton.interactable = true;
    }

    public void HideCollection()
    {
        collectionPanel.SetActive(false);
        foreach (GameObject page in monsterList)
        {
            page.SetActive(false);
        }
        monsterList[0].SetActive(true);
        activeCollectionPage = 0;
        foreach (Image paginationImage in pagination)
        {
            paginationImage.sprite = inactivePage;
        }
        pagination[0].sprite = activePage;
    }

    public void ShowMonster(Sprite image, string monsterName)
    {
        monsterImage.sprite = image;

        Monster viewedMonster = MonsterCollector.sharedInstance.unlockedMonsters[monsterName];

        string localizedName = LocalizationManager.sharedInstance.localizedText["name"];
        string localizedStrength = LocalizationManager.sharedInstance.localizedText["strength"];
        string localizedIntelligence = LocalizationManager.sharedInstance.localizedText["intelligence"];
        string localizedLife = LocalizationManager.sharedInstance.localizedText["life"];

        monsterInfo.GetChild(0).GetComponent<Text>().text = localizedName + ": " + viewedMonster.Name;
        monsterInfo.GetChild(1).GetComponent<Text>().text = localizedStrength + ": " + viewedMonster.Strength;
        monsterInfo.GetChild(2).GetComponent<Text>().text = localizedIntelligence + ": " + viewedMonster.Intelligence;
        monsterInfo.GetChild(3).GetComponent<Text>().text = localizedLife + ": " + viewedMonster.Life;

        monsterView.SetActive(true);

    }

    public void HideMonster()
    {
        monsterView.SetActive(false);
    }

    public void ResetList()
    {

        foreach (GameObject page in monsterList)
        {
            for (int i = 0; i < page.transform.childCount; i++)
            {
                page.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        monsterCollectionPopulated = false;
    }

}
