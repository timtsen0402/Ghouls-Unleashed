using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
using static GameController;
using static PlayerHealth;
using static GameConstants;

public class RoundManager : MonoBehaviour
{
    [Header("Zombie Types")]
    public GameObject z_normal;
    public GameObject z_evolved;
    public GameObject z_grenade;
    public GameObject z_undying;
    [Header("Level Setting")]
    public int level;
    public int winLevel;

    float currentTime;
    int timeConditionCount;

    // s time(start)
    bool s = true;
    // a time(ready)
    bool a = true;
    // b time(battle)
    bool b = true;
    // c time(buffer)
    bool c = true;


    void Start()
    {
        timeConditionCount = 0;
        //level = 1;
    }

    void Update()
    {
        levelTxt.text = level.ToString();

        // (start)
        if (timeConditionCount == 0)
        {
            if (s)
            {
                s = false;
                currentTime = START_TIME;
            }
            CountdownTimer();
            if (currentTime <= 1f)
            {
                timeConditionCount++;
            }
        }
        // (ready)
        else if (timeConditionCount % 3 == 1)
        {
            c = true;
            if (a)
            {
                a = false;
                currentTime = READY_TIME;
                player.transform.position = PLAYER_SPAWN_POS;
                currentHealth = PLAYER_START_HEALTH;
                shop.SetActive(true);
                player.GetComponent<PlayerMovement>().enabled = false;
                FindObjectOfType<AudioManager>().Play("readyBGM");
            }
            CountdownTimer();
            if (currentTime <= 1f)
            {
                timeConditionCount++;
            }
        }
        // (battle)
        else if (timeConditionCount % 3 == 2)
        {
            a = true;
            if (b)
            {
                b = false;
                currentTime = BATTLE_TIME;
                shop.SetActive(false);
                timeTempZ_Normal = 0f;
                timeTempZ_Evolved = 0f;
                timeTempZ_Grenade = 0f;
                player.GetComponent<PlayerMovement>().enabled = true;
                FindObjectOfType<AudioManager>().Play("zombiesScreaming");
                FindObjectOfType<AudioManager>().Stop("readyBGM");
                if (level % 5 == 0)
                    FindObjectOfType<AudioManager>().Play("specialBGM");
                else
                    FindObjectOfType<AudioManager>().Play("normalBGM");
            }
            //進入第level關
            RoundSetting();
            CountdownTimer();
            if (currentTime <= 1f)
            {
                FindObjectOfType<AudioManager>().Play("clear");
                DestroyAllEnemies();
                //顯示通關畫面
                timeConditionCount++;
            }
        }
        // (buffer)
        else
        {
            b = true;
            if (c)
            {
                c = false;
                if (level % 5 == 0)
                    FindObjectOfType<AudioManager>().Stop("specialBGM");
                else
                    FindObjectOfType<AudioManager>().Stop("normalBGM");
                currentTime = BUFFER_TIME;
            }
            CountdownTimer();
            if (currentTime <= 1f)
            {
                if (winLevel == level && !isWin)
                {
                    isWin = true;
                    victory.SetActive(true);
                    FindObjectOfType<AudioManager>().Play("victory");
                    gameObject.GetComponent<RoundManager>().enabled = false;
                }
                else
                {
                    level++;
                    timeConditionCount++;
                }

            }
        }
    }

    void CountdownTimer()
    {
        currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void DestroyAllEnemies()
    {
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Zombie"));

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Target>().Die();
        }
    }
    void RoundSetting()
    {
        switch (level)
        {
            // normal zombie only
            case 1:
                if (timer > timeTempZ_Normal)
                {
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_2, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_3, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_4, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_5, Quaternion.identity);
                    timeTempZ_Normal = timer + 10;
                }
                break;
            // normal zombie more
            case 2:
                if (timer > timeTempZ_Normal)
                {
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_2, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_3, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_4, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_5, Quaternion.identity);
                    timeTempZ_Normal = timer + 7;
                }
                break;
            // normal + evolved zombie
            case 3:
                if (timer > timeTempZ_Normal)
                {
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_2, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_3, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_4, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_5, Quaternion.identity);
                    timeTempZ_Normal = timer + 10;
                }
                if (timer > timeTempZ_Evolved)
                {
                    NavMeshAgent.Instantiate(z_evolved, EVOLVED_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_evolved, EVOLVED_SPAWN_POS_2, Quaternion.identity);
                    timeTempZ_Evolved = timer + 15;
                }
                break;
            // normal + evolved + grenade zombie
            case 4:
                if (timer > timeTempZ_Normal)
                {
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_2, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_3, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_4, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, NORMAL_SPAWN_POS_5, Quaternion.identity);
                    timeTempZ_Normal = timer + 7;
                }
                if (timer > timeTempZ_Evolved)
                {
                    NavMeshAgent.Instantiate(z_evolved, EVOLVED_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_evolved, EVOLVED_SPAWN_POS_2, Quaternion.identity);
                    timeTempZ_Evolved = timer + 20;
                }
                if (timer > timeTempZ_Grenade)
                {
                    NavMeshAgent.Instantiate(z_grenade, BOMB_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, BOMB_SPAWN_POS_2, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, BOMB_SPAWN_POS_3, Quaternion.identity);
                    timeTempZ_Grenade = Mathf.Infinity;
                }
                break;
            // headless + grenade zombie 
            case 5:
                if (timer > timeTempZ_Undying)
                {
                    NavMeshAgent.Instantiate(z_undying, NORMAL_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_undying, EVOLVED_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_undying, EVOLVED_SPAWN_POS_2, Quaternion.identity);
                    timeTempZ_Undying = Mathf.Infinity;
                }
                if (timer > timeTempZ_Grenade)
                {
                    NavMeshAgent.Instantiate(z_grenade, BOMB_SPAWN_POS_1, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, BOMB_SPAWN_POS_2, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, BOMB_SPAWN_POS_3, Quaternion.identity);
                    timeTempZ_Grenade = Mathf.Infinity;
                }
                break;
            case 6:
                print(level);
                break;
            case 7:
                print(level);
                break;
            case 8:
                print(level);
                break;
            case 9:
                print(level);
                break;
            case 10:
                print(level);
                break;
        }
    }
    // void EnemySpawn(GameObject enemy, Vector3 position, float spawnRate)
    // {
    //     if (timer > timeTemp)
    //     {
    //         NavMeshAgent.Instantiate(enemy, position, Quaternion.identity);
    //         timeTemp = timer + spawnRate;
    //     }
    // }
}
