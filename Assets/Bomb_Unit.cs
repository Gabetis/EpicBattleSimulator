using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bomb_Unit : MonoBehaviour
{
    [Header("Cài đặt Bom")]
    public float explosionRadius = 3f; 
    public float explosionDamage = 100f;
    public GameObject explosionEffect; 
    
    private Unit unit;
    private NavMeshAgent agent;
    private bool hasExploded = false;

    void Awake()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (hasExploded) return;

        if (unit.lives <= 0)
        {
            Explode(0f);
            return;
        }

        if (unit.currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, unit.currentTarget.position);
            if (distance <= agent.stoppingDistance + 0.5f)
            {
                Explode(0f);
            }
        }
    }

    public void Explode(float time)
    {
        if (hasExploded) return;
        hasExploded = true;

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
            GameObject fx = Instantiate(explosionEffect, transform.position + Vector3.up, Quaternion.identity);
            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play(true);
        }

        // 3. Chết
        unit.lives = 0;
        StartCoroutine(unit.die());
    }
}