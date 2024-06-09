using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class throwGrenadeAI : MonoBehaviour
{
    [Header("Grenade Type")]
    public GameObject zombieGrenade;

    [Header("Throwing")]
    public float throwForce;
    public float throwFrequency;

    Animator zombieAnime;

    float y_weight = 1f;
    public float timer;
    public float timeTemp;

    void Start()
    {
        zombieAnime = gameObject.GetComponent<Animator>();
        timeTemp = 0f;
    }
    void Update()
    {
        // y_weight = 1
        // y = x * a + b (a:slope,b:intercept)
        throwForce = Vector3.Distance(transform.position, player.transform.position) * 0.13349405f + 7.142484054040478f;
        //print(Vector3.Distance(transform.position, player.transform.position));
        timer = Time.time;

        transform.LookAt(player.transform.position);

        Throw();
    }
    void Throw()
    {
        Vector3 forwardToPlayer = player.transform.position - new Vector3(transform.position.x, transform.position.y + 6, transform.position.z);
        forwardToPlayer = forwardToPlayer.normalized;


        //一定頻率丟 
        if (timer > timeTemp)
        {
            if (timer > timeTemp + 1f) timeTemp = timer;
            timeTemp += throwFrequency;

            zombieAnime.SetBool("isThrow", false);
            zombieAnime.SetBool("isThrow", true);
            zombieAnime.Play("Z_Attack", 0, 0);

            GameObject z_grenade = Instantiate(zombieGrenade, new Vector3(transform.position.x, transform.position.y + 6f, transform.position.z), transform.rotation);
            Rigidbody rb = z_grenade.GetComponent<Rigidbody>();

            Vector3 direction = new Vector3(forwardToPlayer.x, y_weight, forwardToPlayer.z);
            rb.AddForce(direction * throwForce, ForceMode.Impulse);


        }
    }
}

