using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCollection : MonoBehaviour {

    public Transform[] spawnPoints;

    // References to the Monster Collection List
    public GameObject collectionPanel;
    public GameObject monsterButtonPrefab;
    public GameObject monsterList;

    // When a players clicks on a monster, this screen appears
    public GameObject monsterView;
    public Text monsterNameTitle;
    public Image monsterImage;
    public Transform monsterInfo;

    private int spawnedMonster = 0;
    private bool monsterCollectionPopulated = false;

	// Use this for initialization
	void Start () {

		foreach(Monster monster in MonsterCollector.sharedInstance.unlockedMonsters.Values)
        {
            GameObject monsterToInstantiate = MonsterCollector.sharedInstance.monsterPrefabsList[monster.PrefabName];
            
            monsterToInstantiate.GetComponent<MonsterAttributes>().SetAttributes(monster);

            Instantiate(monsterToInstantiate, spawnPoints[spawnedMonster].position, Quaternion.identity, gameObject.transform);

            spawnedMonster++;
            if(spawnedMonster == 4)
            {
                break;
            }
        }
	}

    public void ShowCollection()
    {
        if (!monsterCollectionPopulated)
        {
            foreach (Monster monster in MonsterCollector.sharedInstance.unlockedMonsters.Values)
            {
                foreach (Sprite image in MonsterCollector.sharedInstance.monstersImages)
                {
                    if (monster.PrefabName == image.name)
                    {
                        monsterButtonPrefab.transform.GetChild(0).GetComponent<Image>().sprite = image;
                        monsterButtonPrefab.transform.GetChild(1).GetComponent<Text>().text = monster.Name;
                        monsterButtonPrefab.GetComponent<MonsterViewButton>().monsterCollection = this;
                        Instantiate(monsterButtonPrefab, monsterList.transform);
                    }
                }
            }
            float numberOfMonsters = MonsterCollector.sharedInstance.unlockedMonsters.Count + 0.2f;
            float heightOfList = Mathf.Round(numberOfMonsters / 2) * 650f;

            Debug.Log("numberOfMonsters: " + numberOfMonsters);
            Debug.Log("heightOfList:" + heightOfList);

            RectTransform rt = monsterList.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, heightOfList);

            Debug.Log(rt.sizeDelta);

            monsterCollectionPopulated = true;
        }

        collectionPanel.SetActive(true);
        
    }

    public void HideCollection()
    {
        collectionPanel.SetActive(false);
    }

    public void ShowMonster(Sprite image, string monsterName)
    {
        monsterNameTitle.text = monsterName;
        monsterImage.sprite = image;

        Monster viewedMonster = MonsterCollector.sharedInstance.unlockedMonsters[monsterName];

        monsterInfo.GetChild(0).GetComponent<Text>().text = "Name : " + viewedMonster.Name;
        monsterInfo.GetChild(1).GetComponent<Text>().text = "Strength : " + viewedMonster.Strength;
        monsterInfo.GetChild(2).GetComponent<Text>().text = "Intelligence : " + viewedMonster.Intelligence;
        monsterInfo.GetChild(3).GetComponent<Text>().text = "Life : " + viewedMonster.Life;

        monsterView.SetActive(true);

    }

    public void HideMonster()
    {
        monsterView.SetActive(false);
    }

}
