using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighting : MonoBehaviour {


    private int difficulty;

	public void ChooseMonsterToFight(string monsterName)
    {
        MonsterCollector.sharedInstance.SetMonsterToFight(monsterName, difficulty);
        GameController.sharedInstance.ChangeScene("Fight");
    }
}
