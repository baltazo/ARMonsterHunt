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

    private WaitForSeconds firstWaitFight = new WaitForSeconds(5f);
    private WaitForSeconds waitAttack = new WaitForSeconds(1.5f);

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

        enemyMonster = new Monster("Enemy Monster", "", _str, _intel, _life);

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

        enemyMonster.PrefabName = enemyMonsterPrefab.name;

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
        StartCoroutine(Fight(firstWaitFight));
        // This will go on until one the the two monster's life is 0 or less
    }

    private IEnumerator Fight(WaitForSeconds waitTime) // This sets the animation, when the colliders will touch the monster will actually attack
    {

        yield return waitTime;

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

        // Attacking now
        Debug.Log("Resolving Attack");

        if (playerAttacking)
        {
            // The player attacks the monster
            enemyAnimator.SetTrigger("Damage");
            playerAttacking = false;
        }
        else
        {
            // The monster attacks the player
            playerAnimator.SetTrigger("Damage");
            playerAttacking = true;
        }
    }

    public void DoneAttacking()
    {
        //Done attacking, next turn
        Debug.Log("Done attacking");
        StartCoroutine(Fight(waitAttack));
    }


    
}
