using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_unit : MonoBehaviour
{
    public float AOERadius = 3f; 
    private float AOEDamage;
    public GameObject AOEEffect;

    public Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
        AOEDamage = unit.damage;
    }

    public void AOEAttack(float time)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AOERadius);
        
        foreach (Collider hit in colliders)
        {
            Unit targetUnit = hit.GetComponent<Unit>();
            
            if (targetUnit != null && targetUnit.gameObject.tag != gameObject.tag)
            {
                targetUnit.lives -= AOEDamage;
            }
        }

        if (AOEEffect != null)
        {
            Instantiate(AOEEffect, transform.position, Quaternion.identity);
        }
    } 

    private IEnumerator AOEAttackDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
