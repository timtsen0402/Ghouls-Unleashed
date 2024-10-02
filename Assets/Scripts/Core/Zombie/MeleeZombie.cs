using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerHealth;

public class MeleeZombie : Zombie
{
    [Header("Performance")]
    public float attackRange;
    public float attackDamage;

    protected override void Update()
    {
        TracePlayer();
    }
    void TracePlayer()
    {
        if (health > 0f)
            agent.SetDestination(GameController.player.transform.position);
        else
            agent.SetDestination(transform.position);

        if (Vector3.Distance(GameController.player.transform.position, transform.position) < attackRange)
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
        zombieAnime.Play("Z_Attack");
        FindObjectOfType<AudioManager>().Play("ough");
        currentHealth -= attackDamage;
    }
}
