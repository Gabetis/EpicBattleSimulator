using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour {
    
    public GameObject arrow;
    public Transform arrowSpawner;
    public GameObject animationArrow;
    
    private Animator animator;
    
    void Start(){
        animator = GetComponent<Animator>();
    }
    
    void Update(){
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if(state.IsName("Attacking") && state.normalizedTime % 1 > 0.25f && state.normalizedTime % 1 < 0.95f){
            animationArrow.SetActive(true);
        }
        else{
            animationArrow.SetActive(false);
        }
    }

    // Unit.cs gọi khi attackTimer hết hạn, y hệt melee
	public void Shoot()
	{
		GameObject newArrow = Instantiate(
			arrow,
			arrowSpawner.position,
			arrowSpawner.rotation
		);

		Collider archerCollider = GetComponent<Collider>();
		Collider arrowCollider = newArrow.GetComponent<Collider>();

		if (archerCollider != null && arrowCollider != null)
			Physics.IgnoreCollision(archerCollider, arrowCollider);

		Arrow arrowScript = newArrow.GetComponent<Arrow>();
		AOE_Project aoeArrow = newArrow.GetComponent<AOE_Project>();

		if (arrowScript != null)
		{
			arrowScript.arrowOwner = gameObject;
			arrowScript.arrowDMG = GetComponent<Unit>().damage;
			arrowScript.target = GetComponent<Unit>().currentTarget;
		}
		else if (aoeArrow != null)
		{
			aoeArrow.arrowOwner = gameObject;
			aoeArrow.explosionDamage = GetComponent<Unit>().damage;
			aoeArrow.target = GetComponent<Unit>().currentTarget;
		}
	}
}