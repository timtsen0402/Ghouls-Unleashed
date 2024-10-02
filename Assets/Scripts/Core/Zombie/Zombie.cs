using UnityEngine;
using UnityEngine.AI;
using static PlayerHealth;
using static GameController;

public abstract class Zombie : MonoBehaviour
{
    [Header("Basic Info")]
    public float health = 100f;
    public int value;

    protected Animator zombieAnime;
    protected NavMeshAgent agent;

    protected bool isDie = false;
    protected bool isAttack = false;

    protected AudioSource[] zombieSound;
    protected const int SOUND_WALK = 0;
    protected const int SOUND_HURT = 1;
    protected const int SOUND_DIE = 2;

    protected virtual void Start()
    {
        zombieAnime = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zombieSound = GetComponents<AudioSource>();
    }

    protected virtual void Update()
    {
        if (health <= 0f && !isDie)
        {
            isDie = true;
            OnZombieDeath();
            Die();
        }
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        zombieSound[SOUND_HURT].Play();
    }

    protected virtual void Die()
    {
        // Die Animation
        zombieAnime.SetBool("isDie", true);

        // Turn off all colliders
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider cld in colliders)
        {
            cld.enabled = false;
        }
        // Sound Effects
        zombieSound[SOUND_WALK].Stop();
        zombieSound[SOUND_HURT].Stop();
        zombieSound[SOUND_DIE].Play();

        // Destroy
        Destroy(gameObject, 1f);
    }

    protected void OnZombieDeath()
    {
        money += value;
        killed++;
    }
}