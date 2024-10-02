using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static TradeSystem;

public class Sniper : Gun
{
    float rayLength = Mathf.Infinity; // 射线长度

    LineRenderer lineRenderer;

    [Header("Effect")]
    public Transform gunHead;

    Animator animator;
    Animator recoilAni;

    protected override void Start()
    {
        recoilAni = gameObject.GetComponent<Animator>();

        lineRenderer = GetComponent<LineRenderer>();
    }
    protected override void Shoot()
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
