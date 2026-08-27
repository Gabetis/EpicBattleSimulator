using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Project : MonoBehaviour
{
    [HideInInspector]
    public GameObject arrowOwner;

    [HideInInspector]
    public float arrowDMG;

    [HideInInspector]
    public Transform target;

    public float speed = 40f;

    private string archerTag;
    private Rigidbody rb;

    [Header("Cài đặt Nổ AoE")]
    public float explosionRadius = 3f;
    public float explosionDamage = 50f;
    public GameObject explosionEffect;

    void Start()
    {
        if (arrowOwner != null)
            archerTag = arrowOwner.tag;

        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        if (target != null && !rb.isKinematic)
        {
            rb.useGravity = false;

            Vector3 aimPoint = target.position + new Vector3(0, 1f, 0);
            Vector3 direction = aimPoint - transform.position;

            transform.rotation = Quaternion.LookRotation(direction);
            rb.velocity = transform.forward * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Không va vào chính Mage bắn ra
        if (arrowOwner != null && other.gameObject == arrowOwner)
            return;

        if ((other.CompareTag("Enemy") || other.CompareTag("Knight")) &&
            other.gameObject.tag != archerTag)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            Explode();
        }
        else if (other.CompareTag("Battle ground"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        foreach (Collider hit in colliders)
        {
            Unit targetUnit = hit.GetComponent<Unit>();

            if (targetUnit == null)
                continue;

            // Không đánh chính owner
            if (arrowOwner != null && targetUnit.gameObject == arrowOwner)
                continue;

            // Không đánh quân cùng phe
            if (arrowOwner != null && targetUnit.gameObject.tag == arrowOwner.tag)
                continue;

            targetUnit.lives -= explosionDamage;
        }

        if (explosionEffect != null)
        {
            Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}