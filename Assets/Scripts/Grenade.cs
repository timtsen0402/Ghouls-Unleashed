using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static PlayerHealth;

public class Grenade : MonoBehaviour
{
    public float throwForce = 5f;
    public float explosionRadius = 5f;
    public int damage = 100;
    public float delay = 3f;
    float countDown;
    bool isThrowed = false;
    public GameObject explosionEffect;
    Rigidbody rb;


    void Start()
    {
        countDown = delay;
        rb = gameObject.GetComponent<Rigidbody>();

    }

    void Update()
    {
        //print((int)countDown);
        if (Input.GetButton("Fire1") && !isThrowed && !panel.activeSelf && !shop.activeSelf)
        {
            isThrowed = true;
            gameObject.transform.SetParent(null);
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Rigidbody>().AddForce(fpsCamera.transform.forward * throwForce);

        }
        if (isThrowed)
        {
            countDown -= Time.deltaTime;
        }
        if (countDown <= 0f)
        {
            Explode();
        }
    }
    void Explode()
    {
        FindObjectOfType<AudioManager>().Play("explosion");
        //粒子特效
        Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        //範圍傷害
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider c in colliders)
        {
            if (c.name == "Player")
            {
                currentHealth -= 30;
            }
            if (c.CompareTag("Body"))
            {
                c.GetComponentInParent<Target>().TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
