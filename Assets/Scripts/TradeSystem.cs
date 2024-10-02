using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerHealth;
using static GameController;
using UnityEngine.UI;

public class TradeSystem : MonoBehaviour
{
    public static float attackCoefficient = 1.0f;

    [Header("Weapon Types")]
    public GameObject sniperPrefab;
    public GameObject riflePrefab;
    public GameObject pistolPrefab;
    public GameObject grenadePrefab;
    public GameObject batPrefab;
    [Header("Enhancement Volume")]
    public float attackVolume;
    public int healthVolume;

    int percentageAttack = 100;
    int percentageHealth = 0;

    bool isSniperPurchased;
    bool isRiflePurchased;
    bool isPistolPurchased;
    bool isBatPurchased;

    GameObject weaponHolder;

    void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder");
        attackCoefficient = 1.0f;
    }
    void Update()
    {
        percentageAttackTxt.text = percentageAttack.ToString() + "%";
        percentageHealthTxt.text = "+" + percentageHealth.ToString();
    }

    public void FillupBullet(int price)
    {
        //非槍枝無反應
        if (player.GetComponentInChildren<Gun>() == null && player.GetComponentInChildren<Sniper>() == null)
            return;


        if (player.GetComponentInChildren<Gun>() != null && player.GetComponentInChildren<Gun>().totalAmmo < player.GetComponentInChildren<Gun>().originalTotalAmmo && money >= price)
        {
            player.GetComponentInChildren<Gun>().totalAmmo = player.GetComponentInChildren<Gun>().originalTotalAmmo;
        }

        FindObjectOfType<AudioManager>().Play("refill");
        money -= price;

    }
    public void AttackEnhancement(int price)
    {
        if (money >= price)
        {
            FindObjectOfType<AudioManager>().Play("ping");
            attackCoefficient += attackVolume;
            percentageAttack += (int)(attackVolume * 100);
            money -= price;
        }
    }
    public void HealthEnhancement(int price)
    {
        if (money >= price)
        {
            FindObjectOfType<AudioManager>().Play("ping");

            maxHealth += healthVolume;
            percentageHealth += healthVolume;

            money -= price;
        }
    }
    public void purchaseSniper(int price)
    {
        if (money >= price && !isSniperPurchased)
        {
            FindObjectOfType<AudioManager>().Play("purchase");

            GameObject sniper = Instantiate(sniperPrefab);

            sniper.transform.SetParent(weaponHolder.transform, true);
            sniper.transform.SetSiblingIndex(0);
            sniper.transform.localPosition = new Vector3(-0.47f, 0.24f, -0.39f);
            sniper.transform.localRotation = new Quaternion(0, 1f, 0, -0.06f);
            sniper.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            isSniperPurchased = true;
            money -= price;
        }
    }
    public void purchaseRifle(int price)
    {
        if (money >= price && !isRiflePurchased)
        {
            FindObjectOfType<AudioManager>().Play("purchase");

            GameObject rifle = Instantiate(riflePrefab);

            rifle.transform.SetParent(weaponHolder.transform, true);
            rifle.transform.SetSiblingIndex(0);
            rifle.transform.localPosition = new Vector3(-0.329956055f, -0.0321369395f, -0.426330566f);
            rifle.transform.localRotation = new Quaternion(-0.032893572f, 0, 0, 0.999458849f);
            rifle.transform.localScale = new Vector3(1, 1.8f, 1);

            isRiflePurchased = true;
            money -= price;
        }
    }
    public void purchasePistol(int price)
    {
        if (money >= price && !isPistolPurchased)
        {
            FindObjectOfType<AudioManager>().Play("purchase");

            GameObject pistol = Instantiate(pistolPrefab);
            pistol.transform.SetParent(weaponHolder.transform, true);

            pistol.transform.localPosition = new Vector3(-0.33f, 0.15f, -0.43f);
            pistol.transform.localRotation = new Quaternion(-0.033f, 0f, 0f, 1f);
            pistol.transform.localScale = new Vector3(1, 1.79654121f, 1.00345862f);

            isPistolPurchased = true;
            money -= price;
        }

    }
    public void purchaseGrenade(int price)
    {
        if (money >= price && GameObject.Find("WeaponHolder/Grenade(Clone)") == null)
        {
            FindObjectOfType<AudioManager>().Play("purchase");

            GameObject grenade = Instantiate(grenadePrefab);

            grenade.transform.SetParent(weaponHolder.transform, true);

            grenade.transform.localPosition = new Vector3(0, -0.76f, 0.55f);
            grenade.transform.localRotation = new Quaternion(0, 0.496822447f, 0, -0.867852271f);
            grenade.transform.localScale = new Vector3(12f, 12f, 12f);

            money -= price;
        }

    }
    public void purchaseBat(int price)
    {
        if (money >= price && !isBatPurchased)
        {
            FindObjectOfType<AudioManager>().Play("purchase");

            GameObject bat = Instantiate(batPrefab);

            bat.transform.SetParent(weaponHolder.transform, true);

            bat.transform.localPosition = new Vector3(-0.23f, -0.023f, -0.237f);
            bat.transform.localRotation = new Quaternion(0.00229f, -0.62f, 0.09f, 0.78f);
            bat.transform.localScale = new Vector3(1f, 1f, 1f);

            isBatPurchased = true;
            money -= price;
        }

    }
}
