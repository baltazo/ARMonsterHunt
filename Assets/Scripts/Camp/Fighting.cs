using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighting : MonoBehaviour {

    public GameObject fightMonsterPanel;

    private string chosenMonster;

	public void ChooseMonsterToFight(string monsterName)
    {
        chosenMonster = monsterName;
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
