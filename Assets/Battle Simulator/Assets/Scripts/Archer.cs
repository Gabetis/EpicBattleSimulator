using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour {
	
	public GameObject arrow;
	public Transform arrowSpawner;
	public GameObject animationArrow;
	
	private bool shooting;
	private bool addArrowForce;
	private GameObject newArrow;
	private float shootingForce;
	private Animator animator;
	
	void Start(){
		animator = GetComponent<Animator>();
	}
	
	void Update(){
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		
		if(state.IsName("Attacking") && state.normalizedTime % 1 >= 0.95f && !shooting){
			StartCoroutine(shoot());
		}
		
		if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.25f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < 0.95f){
			animationArrow.SetActive(true);
		}
		else{
			animationArrow.SetActive(false);
		}
	}
	
	void LateUpdate(){
		if(addArrowForce && this.gameObject != null && GetComponent<Unit>().currentTarget != null && newArrow != null && arrowSpawner != null){
			shootingForce = Vector3.Distance(transform.position, GetComponent<Unit>().currentTarget.transform.position);
			newArrow.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(new Vector3(0, shootingForce * 12 + 
			((GetComponent<Unit>().currentTarget.transform.position.y - transform.position.y) * 45), shootingForce * 55)));
			addArrowForce = false;
		}
	}
	
	IEnumerator shoot(){
		shooting = true;
		
		newArrow = Instantiate(arrow, arrowSpawner.position, arrowSpawner.rotation) as GameObject;
		newArrow.GetComponent<Arrow>().arrowOwner = this.gameObject;
		addArrowForce = true;
	
		float currentAttackSpeed = GetComponent<Unit>().attackSpeed;
		yield return new WaitForSeconds(0.5f / Mathf.Max(currentAttackSpeed, 0.1f));
		shooting = false;	
	}
}