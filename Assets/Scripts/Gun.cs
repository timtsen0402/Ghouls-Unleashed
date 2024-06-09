
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameController;
using static TradeSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Performance")]
    public string gunSound;
    public float damage;
    public int totalAmmo;
    public int maxAmmo;
    public float shootRate;
    public float reloadTime = 3f;

    [Header("Gun Effect")]

    public ParticleSystem flash;
    public GameObject impactHole;
    public GameObject impactBlood;
    public GameObject impactBloodHeavy;
    Animator animator;


    public int originalTotalAmmo;
    int currentAmmo;
    float shootRateTimeStamp;
    bool isReloading = false;

    void Start()
    {
        animator = GameObject.Find("WeaponHolder").GetComponent<Animator>();

        currentAmmo = maxAmmo;
        originalTotalAmmo = totalAmmo;
    }
    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("isReloading", false);
    }
    void Update()
    {
        ammoTxt.text = currentAmmo.ToString() + " / " + totalAmmo.ToString();


        if (isReloading)
            return;

        if (Input.GetButton("Fire1") && !panel.activeSelf && !shop.activeSelf)
        {
            if (currentAmmo != 0)
                Shoot();
            else
                FindObjectOfType<AudioManager>().Play("jam");
        }
        if ((currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R)) && totalAmmo != 0 && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.green);
    }

    void Shoot()
    {
        // 可射擊頻率
        if (Time.time > shootRateTimeStamp)
        {
            currentAmmo--;
            flash.Play();
            FindObjectOfType<AudioManager>().PlayGun(gunSound);
            animator.Play("GunShoot", -1, 0f);
            shootRateTimeStamp = Time.time + shootRate;


            RaycastHit hit;
            Vector3 rayStart = fpsCam.transform.position + fpsCam.transform.forward * .7f;
            if (Physics.Raycast(rayStart, fpsCam.transform.forward, out hit))
            {
                print(hit.transform.tag);
                if (hit.transform.name == "Zombie Grenade(Clone)")
                {
                    ZombieGrenade zombieGrenade = hit.transform.gameObject.GetComponent<ZombieGrenade>();
                    zombieGrenade.Ignite();
                }
                Target target = hit.transform.gameObject.GetComponentInParent<Target>();

                if (target != null && target.tag == "Zombie")
                {
                    //爆頭傷害
                    if (hit.transform.name == "Z_Head")
                    {
                        target.TakeDamage(damage * attackCoefficient * 2);
                        Instantiate(impactBloodHeavy, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                    //一般傷害
                    else
                    {
                        target.TakeDamage(damage * attackCoefficient);
                        Instantiate(impactBlood, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
                else
                {
                    GameObject impactGO = Instantiate(impactHole, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 5f);
                }
                //print(hit.transform.name);
            }
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);

        yield return new WaitForSeconds(reloadTime - .8f);
        FindObjectOfType<AudioManager>().Play("reloading");
        animator.SetBool("isReloading", false);
        yield return new WaitForSeconds(.8f);

        if (totalAmmo < maxAmmo)
        {
            if (currentAmmo == 0)
            {
                currentAmmo = totalAmmo;
                totalAmmo = 0;
            }
            else if (currentAmmo + totalAmmo <= maxAmmo)
            {
                currentAmmo += totalAmmo;
                totalAmmo = 0;
            }
            else
            {
                totalAmmo -= maxAmmo - currentAmmo;
                currentAmmo = maxAmmo;
            }
        }
        else
        {
            totalAmmo -= maxAmmo - currentAmmo;
            currentAmmo = maxAmmo;
        }
        isReloading = false;
    }
}
