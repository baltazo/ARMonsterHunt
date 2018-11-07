﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttributes : MonoBehaviour {

    public BattleManager battleManager;

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

    private void OnCollisionEnter(Collision collision) // In the fights, this is called when a monster attacks another
    {
        if (collision.gameObject.tag == "Enemy")
        {
            battleManager.ResolveAttack();
        }
    }
}
