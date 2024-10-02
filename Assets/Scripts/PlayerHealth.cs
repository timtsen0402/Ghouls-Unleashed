using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using static GameController;
using static GameConstants;

public class PlayerHealth : MonoBehaviour
{
    public static float currentHealth;
    public Text txt;
    public LevelLoader levelLoader;
    public GameObject dieFlash;

    bool isLowHP = false;
    PostProcessVolume PPV;

    void Start()
    {
        currentHealth = PLAYER_START_HEALTH;
        PPV = fpsCamera.GetComponent<PostProcessVolume>();
        PPV.enabled = false;
    }
    void Update()
    {
        txt.text = currentHealth.ToString();

        if (currentHealth <= 20f)
        {
            if (currentHealth <= 0f)
            {
                Die();
            }

            if (!isLowHP)
            {
                isLowHP = true;
                PPV.enabled = true;
                FindObjectOfType<AudioManager>().Play("heartBeat");
            }
        }
        else
        {
            isLowHP = false;
            PPV.enabled = false;
            FindObjectOfType<AudioManager>().Stop("heartBeat");
        }

    }
    void Die()
    {
        print("player die");
        Sound[] sounds = gameController.GetComponent<AudioManager>().sounds;
        foreach (Sound s in sounds)
        {
            s.source.volume = 0;
        }

        dieFlash.SetActive(true);
        canvas.SetActive(false);

        levelLoader.OnClickLoadLevel(2);
    }
}