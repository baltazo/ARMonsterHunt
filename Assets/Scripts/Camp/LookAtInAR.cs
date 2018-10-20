using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtInAR : MonoBehaviour {

    public Image monsterImage;

    public void LookAtMonsterInAR()
    {
        string monsterName = monsterImage.sprite.name;
        MonsterCollector.sharedInstance.SetMonsterToLookAt(monsterName);
        GameController.sharedInstance.ChangeScene("MonsterViz");
    }


}
