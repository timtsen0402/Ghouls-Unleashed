using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static PlayerHealth;
using static GameConstants;

public class GameController : MonoBehaviour
{
    #region Public Static
    public static GameObject gameController;
    public static GameObject player;
    public static GameObject fpsCamera;
    public static GameObject canvas;
    public static GameObject panel;
    public static GameObject shop;
    public static GameObject victory;
    public static GameObject warning;

    public static Camera fpsCam;
    public static Image crossHairImg;

    public static Text ammoTxt;
    public static Text killedTxt;
    public static Text timeTxt;
    public static Text levelTxt;
    public static Text moneyTxt;
    public static Text percentageAttackTxt;
    public static Text percentageHealthTxt;



    public static int killed = 0;
    public static int money = 0;
    public static float timer;
    public static float timeTempZ_Normal = 0f;
    public static float timeTempZ_Evolved = 0f;
    public static float timeTempZ_Grenade = 0f;
    public static float timeTempZ_Undying = 0f;

    public static float timeTempPlayerPosLimit = 0f;

    public static bool isWin = false;
    #endregion

    public Material redSky;
    public Material blueSky;


    void Awake() { Loading(); }

    void Start()
    {
        timer = 0f;
        killed = 0;
        money = INITIAL_MONEY;
        panel.SetActive(false);
        shop.SetActive(false);
        victory.SetActive(false);
        warning.SetActive(false);
    }
    void Update()
    {
        PlayerPosLimit();
        CrossHair();
        //TimeCount();
        OnEnableUIPanel();

        timer = Time.time;
        killedTxt.text = killed.ToString();
        moneyTxt.text = "$ " + money.ToString();
    }
    void CrossHair()
    {
        RaycastHit hit;
        Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit);
        if (hit.transform != null)
        {
            if (hit.transform.CompareTag("Target") || hit.transform.CompareTag("Body"))
                crossHairImg.color = Color.red;
            else if (hit.transform.CompareTag("Head"))
                crossHairImg.color = Color.magenta;
            else
                crossHairImg.color = Color.green;
        }
        else
            crossHairImg.color = Color.gray;
    }
    void TimeCount()
    {
        int second = (int)timer;

        int hour = second / 3600;
        int minute = second % 3600 / 60;
        second = second % 3600 % 60;

        timeTxt.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);

    }
    void OnEnableUIPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panel.activeSelf) FindObjectOfType<AudioManager>().Play("turnOff");
            panel.SetActive(!panel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!shop.activeSelf) FindObjectOfType<AudioManager>().Play("turnOn");
            else FindObjectOfType<AudioManager>().Play("turnOff");
            shop.SetActive(!shop.activeSelf);
        }

        if (panel.activeSelf || shop.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            if (panel.activeSelf)
            {
                Time.timeScale = 0f;
                AudioListener.pause = true;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (!panel.activeSelf)
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
            }

        }
        if (isWin)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void PlayerPosLimit()
    {
        if (player.transform.position.x < BOUNDARY_MIN_X || player.transform.position.x > BOUNDARY_MAX_X ||
            player.transform.position.z < BOUNDARY_MIN_Z || player.transform.position.z > BOUNDARY_MAX_Z)
        {
            RenderSettings.skybox = redSky;
            //請回去戰鬥區域
            if (timer > timeTempPlayerPosLimit)
            {
                if (timer > timeTempPlayerPosLimit + 1f) timeTempPlayerPosLimit = timer;
                warning.SetActive(true);
                currentHealth -= 3f;
                FindObjectOfType<AudioManager>().Play("ough");
                timeTempPlayerPosLimit += 1f;
            }
        }
        else
        {
            warning.SetActive(false);
            RenderSettings.skybox = blueSky;

        }
    }
    public void SetSensitivity(float volume)
    {
        MouseLook.mouseSensitivity = volume;
    }

    void Loading()
    {
        gameController = GameObject.Find("Game/Audio Center");
        player = GameObject.Find("Player");
        fpsCamera = GameObject.Find("Main Camera");
        canvas = GameObject.Find("Canvas");
        panel = GameObject.Find("Settings");
        shop = GameObject.Find("Shop");
        warning = GameObject.Find("Warning");

        fpsCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        crossHairImg = GameObject.Find("Canvas/Crosshair").GetComponent<Image>();
        victory = GameObject.Find("Canvas/Victory");
        ammoTxt = GameObject.Find("Canvas/Ammo/Text").GetComponent<Text>();
        killedTxt = GameObject.Find("Canvas/Killed/Text").GetComponent<Text>();
        timeTxt = GameObject.Find("Canvas/Timer").GetComponent<Text>();
        levelTxt = GameObject.Find("Canvas/Level/Text").GetComponent<Text>();
        moneyTxt = GameObject.Find("Money").GetComponent<Text>();
        percentageAttackTxt = GameObject.Find("Attack BTN/Percentage").GetComponent<Text>();
        percentageHealthTxt = GameObject.Find("Health BTN/Percentage").GetComponent<Text>();
    }
}