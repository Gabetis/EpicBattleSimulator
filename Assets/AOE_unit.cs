using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_unit : MonoBehaviour
{
    public float AOERadius = 3f; 
    
    public BoxCollider attackZone;
    
    public GameObject AOEEffect;
    private Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public void AOEAttack(float time)
    {
        float currentDamage = unit.damage;
        
        Collider[] collidersToHit; // Mảng chứa các mục tiêu quét trúng

        if (attackZone != null)
        {
            Vector3 boxCenter = attackZone.transform.TransformPoint(attackZone.center);
            Vector3 halfExtents = Vector3.Scale(attackZone.size, attackZone.transform.lossyScale) * 0.5f;
            
            collidersToHit = Physics.OverlapBox(boxCenter, halfExtents, attackZone.transform.rotation);
        }
        else
        {
            collidersToHit = Physics.OverlapSphere(transform.position, AOERadius);
        }
        
        foreach (Collider hit in collidersToHit)
        {
            Unit targetUnit = hit.GetComponent<Unit>();
            
            if (targetUnit != null && !targetUnit.CompareTag(gameObject.tag))
            {
                targetUnit.lives -= currentDamage;
            }
        }

        if (AOEEffect != null)
        {
            // Nếu có hộp thì sinh hiệu ứng ở giữa hộp, nếu không thì sinh dưới chân con voi
            Vector3 effectPos = (attackZone != null) ? attackZone.transform.TransformPoint(attackZone.center) : transform.position;
            Instantiate(AOEEffect, effectPos, Quaternion.identity);
        }
    } 
}