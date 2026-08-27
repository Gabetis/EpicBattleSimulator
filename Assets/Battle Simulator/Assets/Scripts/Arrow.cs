using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	
	[HideInInspector]
	public GameObject arrowOwner;
	[HideInInspector]
	public float arrowDMG; 
	[HideInInspector]
	public Transform target;
	
	public float speed = 40f; 
	private string archerTag;
	private Rigidbody rb;
	
	void Start(){
		if (arrowOwner != null) archerTag = arrowOwner.tag;
		rb = GetComponent<Rigidbody>();
		Destroy(gameObject, 5f); // 5s không trúng ai thì tự biến mất
	}

	void FixedUpdate(){
		// Code giúp mũi tên bay thẳng về phía mục tiêu
		if (target != null && !rb.isKinematic) {
			rb.useGravity = false; // Tắt trọng lực để không bị rớt bẹt xuống đất
			
			Vector3 aimPoint = target.position + new Vector3(0, 1f, 0); // Nhắm vào ngực
			Vector3 direction = aimPoint - transform.position;
			
			transform.rotation = Quaternion.LookRotation(direction);
			rb.velocity = transform.forward * speed;
		}
	}

	void OnTriggerEnter(Collider other){
		if (arrowOwner != null && other.gameObject == arrowOwner) return;

		if((other.CompareTag("Enemy") || other.CompareTag("Knight")) && other.gameObject.tag != archerTag){
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().isKinematic = true;
			transform.parent = other.gameObject.transform;
			
			Unit victim = other.GetComponent<Unit>();
			if (victim != null) victim.lives -= arrowDMG;
			
			GetComponent<Collider>().enabled = false;
			
			// Tắt sound khi cắm vào người
			if(GetComponent<AudioSource>() != null)
				GetComponent<AudioSource>().Stop();
		}
		else if(other.CompareTag("Battle ground")){
			// Tắt sound khi rớt xuống đất
			if(GetComponent<AudioSource>() != null)
				GetComponent<AudioSource>().Stop();
			Destroy(gameObject);	
		}
	}
}