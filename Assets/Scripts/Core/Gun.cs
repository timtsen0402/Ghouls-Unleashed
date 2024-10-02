
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameController;
using static TradeSystem;

public abstract class Gun : MonoBehaviour
{
    [Header("Gun Performance")]
    [SerializeField] protected string gunSound;
    [SerializeField] protected float damage;
    [SerializeField] public int totalAmmo { private set; get; }
    [SerializeField] public int maxAmmo { private set; get; }
    [SerializeField] protected float shootRate;
    [SerializeField] protected float reloadTime = 3f;

    [Header("Gun Effect")]
    [SerializeField] protected ParticleSystem flash;
    [SerializeField] protected GameObject impactHole;
    [SerializeField] protected GameObject impactBlood;
    [SerializeField] protected GameObject impactBloodHeavy;

    Animator animator;


    protected int originalTotalAmmo;
    protected int currentAmmo;
    protected float shootRateTimeStamp;
    protected bool isReloading = false;

    protected virtual void Start()
    {
        animator = GameObject.Find("WeaponHolder").GetComponent<Animator>();

        currentAmmo = maxAmmo;
        originalTotalAmmo = totalAmmo;
    }
    protected virtual void OnEnable()
    {
        isReloading = false;
        animator.SetBool("isReloading", false);
    }
    protected virtual void Update()
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

    protected virtual void Shoot()
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
                Zombie zombie = hit.transform.gameObject.GetComponentInParent<Zombie>();

                if (zombie != null && zombie.tag == "Zombie")
                {
                    //爆頭傷害
                    if (hit.transform.name == "Z_Head")
                    {
                        zombie.TakeDamage(damage * attackCoefficient * 2);
                        Instantiate(impactBloodHeavy, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                    //一般傷害
                    else
                    {
                        zombie.TakeDamage(damage * attackCoefficient);
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
    protected virtual IEnumerator Reload()
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
