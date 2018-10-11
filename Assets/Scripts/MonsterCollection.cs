using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollection : MonoBehaviour {

    public Transform[] spawnPoints;

    private int spawnedMonster = 0;

	// Use this for initialization
	void Start () {
		foreach(Monster monster in MonsterCollector.sharedInstance.unlockedMonsters)
        {
            GameObject monsterToInstantiate = MonsterCollector.sharedInstance.monsterPrefabsList[monster.PrefabName];

            Instantiate(monsterToInstantiate, spawnPoints[spawnedMonster].position, Quaternion.identity, gameObject.transform);

            spawnedMonster++;
            if(spawnedMonster == 4)
            {
                break;
            }
        }
	}

}
