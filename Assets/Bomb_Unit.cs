using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb_Unit : MonoBehaviour
{
    [Header("Cài đặt Bom")]
    public float explosionRadius = 3f; 
    public float explosionDamage = 100f;
    public GameObject explosionEffect; 
    
    private Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public void Explode(float time)
    {
     StartCoroutine(ExplodeDelayTime(time));

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider hit in colliders)
        {
            Unit targetUnit = hit.GetComponent<Unit>();
            
            if (targetUnit != null && targetUnit.gameObject.tag != gameObject.tag)
            {
                targetUnit.lives -= explosionDamage;
            }
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        StartCoroutine(unit.die());
    }

    public IEnumerator ExplodeDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}