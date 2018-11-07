using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    private GameObject playerMonsterPrefab;
    private Monster playerMonster;
    private int playerLife;
    private int playerStrength;
    private int playerIntel;
    private Animator playerAnimator;

    private GameObject enemyMonsterPrefab;
    private Monster enemyMonster;
    private int difficulty;
    private int enemyLife;
    private int enemyIntel;
    private int enemyStrength;
    private Animator enemyAnimator;

    [SerializeField] private int lifeMultiplier = 10;

    [SerializeField] private int easyAttributes = 100;
    [SerializeField] private int mediumAttributes = 200;
    [SerializeField] private int hardAttributes = 400;

    [SerializeField] private int startFightSeconds = 3;
    [SerializeField] private int attackDelay = 2;

    private bool playerAttacking;

	// Use this for initialization
	void Start () {
        playerMonster = MonsterCollector.sharedInstance.monsterToFight;
        playerLife = playerMonster.Life * lifeMultiplier;
        playerIntel = playerMonster.Intelligence;
        playerStrength = playerMonster.Strength;
        difficulty = MonsterCollector.sharedInstance.difficulty;
        CreateEnemy();
	}
	
	public void CreateEnemy() // Creates an enemy pretty much the same way as a monster for the player
    {
        int[] enemyAttributes = new int[3];

        if (difficulty == 0)
        {
            for (int i = 0; i < easyAttributes; i++)
            {
                int RandomAttribute = Random.Range(0, 3);
                enemyAttributes[RandomAttribute]++;
            }
            for (int i = 0; i < enemyAttributes.Length; i++)
            {
                if (enemyAttributes[i] == 0)
                {
                    enemyAttributes[i]++;
                }
            }
        }
        else if (difficulty == 1)
        {
            for (int i = 0; i < mediumAttributes; i++)
            {
                int RandomAttribute = Random.Range(0, 3);
                enemyAttributes[RandomAttribute]++;
            }
            for (int i = 0; i < enemyAttributes.Length; i++)
            {
                if (enemyAttributes[i] == 0)
                {
                    enemyAttributes[i]++;
                }
            }
        }
        else if (difficulty == 2)
        {

            for (int i = 0; i < hardAttributes; i++)
            {
                int RandomAttribute = Random.Range(0, 3);
                enemyAttributes[RandomAttribute]++;
            }
            for (int i = 0; i < enemyAttributes.Length; i++)
            {
                if (enemyAttributes[i] == 0)
                {
                    enemyAttributes[i]++;
                }
            }
        }

        int _str = enemyAttributes[0];
        int _intel = enemyAttributes[1];
        int _life = enemyAttributes[2];

        enemyMonster = new Monster("Enemy Monster", enemyMonsterPrefab.name, _str, _intel, _life);

        enemyLife = _life * lifeMultiplier;
        enemyStrength = _str;
        enemyIntel = _intel;

    }

    public void StartFight(GameObject player, GameObject enemy)
    {
        playerMonsterPrefab = player;
        enemyMonsterPrefab = enemy;

        playerMonsterPrefab.GetComponent<MonsterAttributes>().battleManager = this;
        enemyMonsterPrefab.GetComponent<MonsterAttributes>().battleManager = this;

        playerAnimator = playerMonsterPrefab.GetComponent<Animator>();
        enemyAnimator = enemyMonsterPrefab.GetComponent<Animator>();

        if (playerIntel >= enemyIntel)
        {
            playerAttacking = true;
        }
        else
        {
            playerAttacking = false;
        }
        InvokeRepeating("Fight", startFightSeconds, attackDelay); // This will go on until one the the two monster's life is 0 or less
    }

    private void Fight() // This sets the animation, when the colliders will touch the monster will actually attack
    {
        if (playerAttacking)
        {
            playerAnimator.SetTrigger("Attack");
        }
        else
        {
            enemyAnimator.SetTrigger("Attack");
        }
    }

    public void ResolveAttack() // This is called by the MonsterAttributes script OnCollisionEnter
    {
        if (playerAttacking)
        {
            // The player attacks the monster
        }
        else
        {
            // The monster attacks the player
        }
    }
    
}
