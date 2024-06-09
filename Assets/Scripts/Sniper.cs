using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static TradeSystem;

public class Sniper : MonoBehaviour
{
    float rayLength = Mathf.Infinity; // 射线长度

    LineRenderer lineRenderer;

    [Header("Performance")]
    public string gunSound;
    public float damage;
    public int totalAmmo;
    public int maxAmmo;
    public float shootRate;
    public float reloadTime = 3f;

    [Header("Effect")]
    public Transform gunHead;
    public ParticleSystem flash;
    public GameObject impactHole;
    public GameObject impactBloodHeavy;
    Animator animator;
    Animator recoilAni;


    public int originalTotalAmmo;
    int currentAmmo;
    float shootRateTimeStamp;
    bool isReloading = false;


    void Start()
    {
        animator = GameObject.Find("WeaponHolder").GetComponent<Animator>();
        recoilAni = gameObject.GetComponent<Animator>();

        currentAmmo = maxAmmo;
        originalTotalAmmo = totalAmmo;

        lineRenderer = GetComponent<LineRenderer>();
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
            recoilAni.Play("Shoot");
            shootRateTimeStamp = Time.time + shootRate;


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayLength);
            foreach (RaycastHit hit in hits)
            {
                // 处理每个命中的物体
                Debug.Log("射线碰撞到了：" + hit.collider.gameObject.name);

                // 更新LineRenderer的位置，只显示到最后一个命中点
                lineRenderer.enabled = true;
                if (hit.transform.name == "Terrain")
                {
                    lineRenderer.SetPosition(0, gunHead.position);
                    lineRenderer.SetPosition(1, hit.point);

                    GameObject impactGO = Instantiate(impactHole, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 5f);
                }
                StartCoroutine(FadeLineRenderer());

                if (hit.transform.name == "Zombie Grenade(Clone)")
                {
                    ZombieGrenade zombieGrenade = hit.transform.gameObject.GetComponent<ZombieGrenade>();
                    zombieGrenade.Ignite();
                }

                Target target = hit.transform.gameObject.GetComponentInParent<Target>();
                if (target != null && target.tag == "Zombie")
                {

                    target.TakeDamage(damage * attackCoefficient);
                    Instantiate(impactBloodHeavy, hit.point, Quaternion.LookRotation(hit.normal));

                }
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
    IEnumerator FadeLineRenderer()
    {
        Gradient lineRendererGradient = new Gradient();
        float fadeSpeed = 1f;
        float timeElapsed = 0f;
        float alpha;

        while (timeElapsed < fadeSpeed)
        {
            alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeSpeed);

            lineRendererGradient.SetKeys
            (
                lineRenderer.colorGradient.colorKeys,
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 1f) }
            );
            lineRenderer.colorGradient = lineRendererGradient;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }

}
