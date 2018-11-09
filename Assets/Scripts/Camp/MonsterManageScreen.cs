using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManageScreen : MonoBehaviour {

    public Training training;
    public Breeding breeding;
    public Fighting fighting;

    public GameObject monsterManagePanel;
    public GameObject[] monsterList;
    public Image titleImage;
    public Text screenTitle;

    public Sprite fusionTitle;
    public Sprite[] trainingTitle;
    public Sprite[] fightTitle;

    public GameObject monsterButtonPrefab;

    public Button previousPageButton;
    public Button nextPageButton;

    public Image[] pagination;
    public Sprite activePage;
    public Sprite inactivePage;

    private bool monsterListPopulated = false;
    private string activeScreen;
    public int activeCollectionPage = 0;
    private int maxPage = 5; // Change this depending on the max number of monster you want

    private void Start()
    {
        GameController.sharedInstance.monsterManage = this;
        monsterList[0].SetActive(true);
        if (MonsterCollector.sharedInstance.unavailableMonsters.Count > 8)
        {
            nextPageButton.interactable = true;
        }
    }

    public void ShowSelectedList(string screenSelected)
    {

        activeScreen = screenSelected;

        Debug.Log(activeScreen);

        if (!monsterListPopulated)
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
                        monsterButtonPrefab.GetComponent<MonsterViewButton>().monsterManage = this;

                        Instantiate(monsterButtonPrefab, monsterList[monsterCollectionPage].transform);
                        buttonsAdded++;
                    }

                    if (buttonsAdded == 8)
                    {
                        monsterCollectionPage++;
                        buttonsAdded = 0;
                        nextPageButton.interactable = true;
                    }
                }
            }

            monsterListPopulated = true;
        }

        foreach (GameObject page in monsterList)
        {
            for (int i = 0; i < page.transform.childCount; i++)
            {
                Transform button = page.transform.GetChild(i);
                string monsterName = page.transform.GetChild(i).GetChild(1).GetComponent<Text>().text;
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
                titleImage.sprite = fusionTitle;
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
                if (LocalizationManager.sharedInstance.isFrench)
                {
                    titleImage.sprite = trainingTitle[1];
                }
                else
                {
                    titleImage.sprite = trainingTitle[0];
                }
                screenTitle.text = LocalizationManager.sharedInstance.localizedText["training"];
                monsterManagePanel.SetActive(true);
            }
        }

        if (activeScreen == "Fighting")
        {
            if (LocalizationManager.sharedInstance.isFrench)
            {
                titleImage.sprite = fightTitle[1];
            }
            else
            {
                titleImage.sprite = fightTitle[0];
            }
            screenTitle.text = LocalizationManager.sharedInstance.localizedText["fighting"];
            monsterManagePanel.SetActive(true);
        }
        
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
        else if (monsterList[activeCollectionPage + 1].transform.childCount == 0)
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
            fighting.ChooseMonsterToFight( image, monster);
        }

        
    }

    public void HideManagePanel()
    {
        monsterManagePanel.SetActive(false);

        if (activeScreen == "Breeding")
        {
            breeding.CancelBreeding();
        }

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

    public void ResetList()
    {
        foreach (GameObject page in monsterList)
        {
            for (int i = 0; i < page.transform.childCount; i++)
            {
                page.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        monsterListPopulated = false;
    }
}
