using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighting : MonoBehaviour {

    public GameObject fightMonsterPanel;
    public Image fightMonsterImage;

    private string chosenMonster;

	public void ChooseMonsterToFight(Sprite image, string monsterName)
    {
        chosenMonster = monsterName;
        fightMonsterImage.sprite = image;
        ShowHideFightViewPanel();
    }

    public void ShowHideFightViewPanel()
    {
        fightMonsterPanel.SetActive(!fightMonsterPanel.activeSelf);
    }

    public void StartFighting(int difficulty)
    {
        MonsterCollector.sharedInstance.SetMonsterToFight(chosenMonster, difficulty);
        GameController.sharedInstance.ChangeScene("Fight");
    }

}
