using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
using static GameController;
using static PlayerHealth;

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
    [Header("Time Setting")]
    public float startTime;
    public float readyTime;
    public float battleTime;
    public float bufferTime;

    float currentTime;
    int timeConditionCount;

    Vector3 spawnPos5 = new(780, 20, 505);
    Vector3 spawnPos6 = new(685, 40, 420);
    Vector3 spawnPos7 = new(560, 5, 450);
    Vector3 spawnPos8 = new(840, 40, 515);
    Vector3 spawnPos9 = new(1000, 25, 555);
    Vector3 spawnPos10 = new(640, 45, 745);

    Vector3 spawnPos11 = new(750, 4, 870);
    Vector3 spawnPos12 = new(740, 3, 470);

    Vector3 spawnPos13 = new(800, 3, 700);
    Vector3 spawnPos14 = new(800, 3, 600);
    Vector3 spawnPos15 = new(600, 3, 650);


    Vector3 center = new(760, 15, 650);
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
                currentTime = startTime;
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
                currentTime = readyTime;
                player.transform.position = playerOringinalPos;
                currentHealth = maxHealth;
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
                currentTime = battleTime;
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
                currentTime = bufferTime;
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
                    NavMeshAgent.Instantiate(z_normal, spawnPos5, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos6, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos7, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos8, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos10, Quaternion.identity);
                    timeTempZ_Normal = timer + 10;
                }
                break;
            // normal zombie more
            case 2:
                if (timer > timeTempZ_Normal)
                {
                    NavMeshAgent.Instantiate(z_normal, spawnPos5, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos6, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos7, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos8, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos10, Quaternion.identity);
                    timeTempZ_Normal = timer + 7;
                }
                //多普疆
                break;
            // normal + evolved zombie
            case 3:
                if (timer > timeTempZ_Normal)
                {
                    NavMeshAgent.Instantiate(z_normal, spawnPos5, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos6, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos7, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos8, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos10, Quaternion.identity);
                    timeTempZ_Normal = timer + 10;
                }
                if (timer > timeTempZ_Evolved)
                {
                    NavMeshAgent.Instantiate(z_evolved, spawnPos11, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_evolved, spawnPos12, Quaternion.identity);
                    timeTempZ_Evolved = timer + 15;
                }
                //普疆+一點快將
                break;
            // normal + evolved + grenade zombie
            case 4:
                if (timer > timeTempZ_Normal)
                {
                    NavMeshAgent.Instantiate(z_normal, spawnPos5, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos6, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos7, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos8, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_normal, spawnPos10, Quaternion.identity);
                    timeTempZ_Normal = timer + 7;
                }
                if (timer > timeTempZ_Evolved)
                {
                    NavMeshAgent.Instantiate(z_evolved, spawnPos11, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_evolved, spawnPos12, Quaternion.identity);
                    timeTempZ_Evolved = timer + 20;
                }
                if (timer > timeTempZ_Grenade)
                {
                    NavMeshAgent.Instantiate(z_grenade, spawnPos13, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, spawnPos14, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, spawnPos15, Quaternion.identity);
                    timeTempZ_Grenade = Mathf.Infinity;
                }
                //普+快+手劉
                break;
            // headless + grenade zombie 
            case 5:
                if (timer > timeTempZ_Undying)
                {
                    NavMeshAgent.Instantiate(z_undying, spawnPos5, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_undying, spawnPos11, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_undying, spawnPos12, Quaternion.identity);
                    timeTempZ_Undying = Mathf.Infinity;
                }
                if (timer > timeTempZ_Grenade)
                {
                    NavMeshAgent.Instantiate(z_grenade, spawnPos13, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, spawnPos14, Quaternion.identity);
                    NavMeshAgent.Instantiate(z_grenade, spawnPos15, Quaternion.identity);
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
