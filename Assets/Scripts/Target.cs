using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GameController;
using static PlayerHealth;

public class Target : MonoBehaviour
{
    [Header("Zombie Performance")]
    public bool tracePlayer;
    public float health = 100f;
    public float attackRange = 5f;
    public float attackDamage;
    [Header("Zombie Effect")]
    public int value;
    public Animator zombieAnime;
    public NavMeshAgent agent;

    bool isDie = false;
    bool isAttack = false;

    // sound No.#
    AudioSource[] zombieSound;
    int walk = 0;
    int hurt = 1;
    int die = 2;

    private void Start()
    {
        zombieSound = gameObject.GetComponents<AudioSource>();
    }
    private void Update()
    {
        if (tracePlayer)
            TracePlayer();

        if (health <= 0f && !isDie)
        {
            isDie = true;
            money += value;
            killed++;
            Die();
        }
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        zombieSound[hurt].Play();
    }

    public void Die()
    {
        // Die Animation
        zombieAnime.SetBool("isDie", true);

        // Turn off all colliders
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider cld in colliders)
        {
            cld.enabled = false;
        }
        // Sound Effects
        zombieSound[walk].Stop();
        zombieSound[hurt].Stop();
        zombieSound[die].Play();

        // Destroy
        Destroy(gameObject, 1f);
    }
    void TracePlayer()
    {
        if (health > 0f) agent.SetDestination(player.transform.position);
        else agent.SetDestination(agent.transform.position);

        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < attackRange)
        {
            if (!isAttack && !isDie)
                Attack();
        }
        else
        {
            isAttack = false;
        }
    }
    void Attack()
    {
        isAttack = true;
        //zombieAnime.SetTrigger("isClose");
        zombieAnime.Play("Z_Attack");
        FindObjectOfType<AudioManager>().Play("ough");
        currentHealth -= attackDamage;

        // player HP reduce once for each attack animation
    }
}
