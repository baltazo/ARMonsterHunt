using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttributes : MonoBehaviour {

    [SerializeField] private string monsterName;
    [SerializeField] private int strength;
    [SerializeField] private int intelligence;
    [SerializeField] private int life;

    public void SetAttributes(Monster thisMonster)
    {
        monsterName = thisMonster.Name;
        strength = thisMonster.Strength;
        intelligence = thisMonster.Intelligence;
        life = thisMonster.Life;
    }
}
