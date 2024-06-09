using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameController;
public class Melee : MonoBehaviour
{
    float hitRateTimeStamp;

    [Header("Melee Performance")]
    public float damage = 50f;
    public float hitRate = 2f;
    public float attackRange = 10f;
    public float attackAngle = 90f;

    [Header("Melee Effect")]
    public Animator animator;
    public GameObject impactBlood;
    public String attackAnimatorName;
    public String hitAudioName;

    void Update()
    {
        if (Input.GetButton("Fire1") && !panel.activeSelf && !shop.activeSelf)
        {
            Hit();
        }
    }
    void Hit()
    {
        if (Time.time > hitRateTimeStamp)
        {
            animator.Play(attackAnimatorName);
            hitRateTimeStamp = Time.time + hitRate;

            Vector3 forwardDirection = fpsCam.transform.forward;

            float halfAngle = attackAngle / 2f;

            Collider[] hitColliders = Physics.OverlapSphere(fpsCam.transform.position, attackRange);

            foreach (Collider hitCollider in hitColliders)
            {
                Vector3 targetDirection = hitCollider.transform.position - fpsCam.transform.position;
                float angle = Vector3.Angle(forwardDirection, targetDirection);

                if (angle <= halfAngle)
                {
                    Target target = hitCollider.transform.gameObject.GetComponentInParent<Target>();
                    if (target != null && target.CompareTag("Zombie"))
                    {
                        target.TakeDamage(damage);
                        FindObjectOfType<AudioManager>().Play(hitAudioName);
                        if (gameObject.name != "Bat")
                            Instantiate(impactBlood, new(target.transform.position.x, target.transform.position.y + 5f, target.transform.position.z), Quaternion.identity);
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("swing");
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // 在Scene视图中绘制扇形攻击范围
        Gizmos.color = Color.red;

        // 获取武器前方的方向
        Vector3 forwardDirection = fpsCam.transform.forward;

        // 计算扇形的半角度
        float halfAngle = attackAngle / 2f;

        // 计算扇形的左边界和右边界
        Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * forwardDirection;
        Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * forwardDirection;

        // 绘制扇形范围
        Gizmos.DrawRay(fpsCam.transform.position, leftBoundary * attackRange);
        Gizmos.DrawRay(fpsCam.transform.position, rightBoundary * attackRange);
    }
}
