using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerHealth;

public class ZombieGrenade : MonoBehaviour
{
    public float explosionRadius = 5f;
    public int damage = 10;

    GameObject zombieAttach;
    Rigidbody rb;
    //AudioSource grenadeSound;
    public GameObject explosionEffect;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        zombieAttach = GameObject.Find("Zombie Bomb");
        //grenadeSound = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        //範圍傷害
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        if (colliders == null) return;
        foreach (Collider c in colliders)
        {
            if (c.name == "Terrain")
            {
                rb.drag = 0.5f;
            }
            if (c.name == "Player")
            {
                FindObjectOfType<AudioManager>().Play("ough");
                currentHealth -= damage;
                Ignite();
            }

        }


    }
    public void Ignite()
    {
        //音效
        FindObjectOfType<AudioManager>().Play("explosion");
        //特效
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
