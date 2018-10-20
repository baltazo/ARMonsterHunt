using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MonsterViewButton : MonoBehaviour {

    public MonsterCollection monsterCollection;

    public void ShowThisMonster()
    {
        string name = gameObject.transform.GetChild(1).GetComponent<Text>().text;
        Sprite image = gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        monsterCollection.ShowMonster(image, name);
    }

}
