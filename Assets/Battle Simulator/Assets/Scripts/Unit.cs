using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine.AI;
using System.Linq;

public class Unit : MonoBehaviour {
	
	//variables visible in the inspector
	public float lives;
	public float damage;
	public float attackSpeed = 1f;
	private float attackTimer = 0f;
	public string attackTag;
	public GameObject ragdoll;
	public AudioClip attackAudio;
	public AudioClip runAudio;
	
	//not visible in the inspector
	[HideInInspector]
	public Transform currentTarget;
	
	[HideInInspector]
	public bool spread;
	
	private NavMeshAgent agent;
	private GameObject health;
	private GameObject healthbar;
	
	[HideInInspector]
	private bool isAttack = false;
	private float startLives;
	private float defaultStoppingDistance;
	private Animator animator;
	private AudioSource source;
	
	private Vector3 randomTarget;
	private WalkArea area;
	
	private ParticleSystem dustEffect;
	private int maxAlliesPerEnemy;
	
	private bool dead;
	
	void Start(){
		//if this an archer or enemy, don't use the spread option
		if(GetComponent<Archer>() || gameObject.tag == "Enemy")
			spread = false;
		
		//get the audio source
		source = GetComponent<AudioSource>();
		maxAlliesPerEnemy = 1;
	
		//find navmesh agent component
		agent = gameObject.GetComponent<NavMeshAgent>();
		animator = gameObject.GetComponent<Animator>();
	
		//find objects attached to this character
		health = transform.Find("Health").gameObject;
		healthbar = health.transform.Find("Healthbar").gameObject;
		health.SetActive(false);	
	
		//set healtbar value
		healthbar.GetComponent<Slider>().maxValue = lives;
		startLives = lives;
		//get default stopping distance
		defaultStoppingDistance = agent.stoppingDistance;
	
		//if there's a dust effect, find and assign it
		if(transform.Find("dust"))
			dustEffect = transform.Find("dust").gameObject.GetComponent<ParticleSystem>();
		
		//find the area so the character can walk around
		area = GameObject.FindObjectOfType<WalkArea>();
	}
	
	void FixedUpdate()
	{
		if (agent != null && (!agent.isActiveAndEnabled || !agent.isOnNavMesh)) return; 

		if (currentTarget != null && !currentTarget.CompareTag(attackTag)) {
			currentTarget = null; 
		}

		if(lives != startLives)
		{
			if(!health.activeSelf)
				health.SetActive(true);

			health.transform.LookAt(2 * transform.position - Camera.main.transform.position);
			healthbar.GetComponent<Slider>().value = lives;
		}

		// Find closest enemy
		if(currentTarget == null && GameObject.FindGameObjectsWithTag(attackTag).Length > 0)
			currentTarget = findCurrentTarget();

		// Die
		if(lives < 1 && !dead)
		{
			StartCoroutine(die());
			return;
		}
		// Dust effect
		if(dustEffect && animator.GetBool("Attacking") == false && !dustEffect.isPlaying)
			dustEffect.Play();

		if(dustEffect && dustEffect.isPlaying && animator.GetBool("Attacking") == true)
			dustEffect.Stop();

		// No target
		if(currentTarget == null)
		{
			walkRandomly();
			return;
		}

		// Default stopping distance
		if(agent.stoppingDistance != defaultStoppingDistance)
			agent.stoppingDistance = defaultStoppingDistance;

		float distance = Vector3.Distance(currentTarget.position, transform.position);

		if(distance > agent.stoppingDistance)
		{
			// Địch ở xa -> chạy
			agent.isStopped = false;
			agent.destination = currentTarget.position;

			animator.SetBool("Attacking", false);
			animator.SetBool("Start", true);
			animator.speed = 1f;

			if(source.clip != runAudio)
			{
				source.clip = runAudio;
				source.Play();
			}

			return;
		}

		agent.isStopped = true;

		Vector3 currentTargetPosition = currentTarget.position;
		currentTargetPosition.y = transform.position.y;
		transform.LookAt(currentTargetPosition);
		animator.speed = 1f;
		animator.SetBool("Start", false); 
		animator.SetBool("Attacking", true); 

		if(attackSpeed <= 0f) return;
		attackTimer -= Time.fixedDeltaTime;

		if(attackTimer > 0f) return;

		if(source.clip != attackAudio)
		{
			source.clip = attackAudio;
			source.Play();
		}
		Unit targetUnit = currentTarget.GetComponent<Unit>();
		if(targetUnit != null)
		{
			Bomb_Unit bomb = GetComponent<Bomb_Unit>();
			
			if (bomb != null) {
				bomb.Explode(attackSpeed);
				return; 
			} else {
				targetUnit.lives -= damage;
			}
		}

		attackTimer = 1f / attackSpeed;
	}

	//randomly walk around
	public void walkRandomly(){
		//check if the area exists
		if(area != null){
			//set the stopping distance to 2
			if(agent.stoppingDistance > 2)
				agent.stoppingDistance = 2;
			
			//get a random position in the area
			if(randomTarget == Vector3.zero || Vector3.Distance(transform.position, randomTarget) < 3f)
				randomTarget = getRandomPosition(area);
			
			//check if the random target is not equal to vector3.zero
			if(randomTarget != Vector3.zero){
				//stop attacking
				if(animator.GetBool("Attacking")){
					animator.SetBool("Attacking", false);

					animator.speed = 1f;

					//play the run audio
					if(source.clip != runAudio){
						source.clip = runAudio;
						source.Play();
					}
				}
				
				//move the agent around
				agent.isStopped = false;
				agent.destination = randomTarget;					
			}
		}
	}
	
	public Transform findCurrentTarget(){  
		//find all potential targets (enemies of this character)
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(attackTag);
		Transform target = null;
		
		//if we want this character to communicate with his allies
		if(spread){
			//get all enemies
			List<GameObject> availableEnemies = enemies.ToList();
			int count = 0;
			
			//make sure it doesn't get stuck in an infinite loop
			while(count < 300){
				//for all enemies
				for(int i = 0; i < enemies.Length; i++){
					//distance between character and its nearest enemy
					float closestDistance = Mathf.Infinity;
		
					foreach(GameObject potentialTarget in availableEnemies){
						//check if there are enemies left to attack and check per enemy if its closest to this character
						if(Vector3.Distance(transform.position, potentialTarget.transform.position) < closestDistance && potentialTarget != null){
							//if this enemy is closest to character, set closest distance to distance between character and enemy
							closestDistance = Vector3.Distance(transform.position, potentialTarget.transform.position);
							target = potentialTarget.transform;
						}
					}	
					
					//if it is valid, return this target
					if(target && canAttack(target)){
						return target;
					}
					else{
						//if it's not, remove it from the list and try again
						availableEnemies.Remove(target.gameObject);
					}
				}
				
				//after checking all enemies, allow one more ally to also attack the same enemy and try again
				maxAlliesPerEnemy++;
				availableEnemies.Clear();
				availableEnemies = enemies.ToList();
			
				count++;
			}
			
			//show a loop error
			Debug.LogError("Infinite loop");
		}
		else{
			//if we're using the simple method:
			float closestDistance = Mathf.Infinity;
		
			foreach(GameObject potentialTarget in enemies){
				//check if there are enemies left to attack and check per enemy if its closest to this character
				if(Vector3.Distance(transform.position, potentialTarget.transform.position) < closestDistance && potentialTarget != null){
					//if this enemy is closest to character, set closest distance to distance between character and enemy
					closestDistance = Vector3.Distance(transform.position, potentialTarget.transform.position);
					target = potentialTarget.transform;
				}
			}	
			
			//check if there's a target and return it
			if(target)
				return target;
		}
		
		//otherwise return null
		return null;
	}
	
	//check if there's not too much allies attacking this same enemy already
	public bool canAttack(Transform target){
		//get the number of allies that are already attacking this enemy
		int numberOfUnitsAttackingThisEnemy = 0;
		
		//foreach ally that's attacking the same enemy, increase the number of allies
		foreach(GameObject ally in GameObject.FindGameObjectsWithTag(gameObject.tag)){
			if(ally.GetComponent<Unit>().currentTarget == target && !ally.GetComponent<Archer>())
				numberOfUnitsAttackingThisEnemy++;
		}
		
		//check if we may attack this target
		if(numberOfUnitsAttackingThisEnemy < maxAlliesPerEnemy)
			return true;
		
		//return false if there's too much allies attacking this enemy already
		return false;
	}
	
	//find a random position inside the walk area
	public Vector3 getRandomPosition(WalkArea area){
		Vector3 center = area.center;
		Vector3 bounds = area.area;
		
		//create a ray using the center and the bounds
		float yRay = center.y + bounds.y/2f;
		
		//get a random position for the ray to start from
		Vector3 rayStart = new Vector3(center.x + Random.Range(-bounds.x/2f, bounds.x/2f), yRay, center.z + Random.Range(-bounds.z/2f, bounds.z/2f));
		
		//store the raycast hit
		RaycastHit hit;
		
		//check if there's terrain underneath
		if(Physics.Raycast(rayStart, -Vector3.up, out hit, bounds.y))
			return hit.point;
		
		//if there's no terrain, return the center
		return Vector3.zero;
	}
	
	public IEnumerator die(){
		dead = true;
		
		if (ragdoll != null) 
		{
			Instantiate(ragdoll, transform.position, transform.rotation);
			yield return new WaitForEndOfFrame();
			Destroy(gameObject);
		}
		else 
		{
			this.enabled = false;	
			// 1. Tắt di chuyển và tắt va chạm để lính khác bước xuyên qua xác
			if (agent != null) {
				if (agent.isOnNavMesh) agent.isStopped = true; 
				agent.enabled = false;
			}
			if (GetComponent<Collider>() != null) {
				GetComponent<Collider>().enabled = false;
			}

			gameObject.tag = "Untagged";
			
			// 2. Kích hoạt Animation chết
			animator.SetTrigger("Die");
			
			// 3. Chờ 3 giây để animation diễn xong rồi từ từ biến mất (Xóa xác)
			// (Bạn có thể tăng lên 5f hoặc 10f tùy thích)
			yield return new WaitForSeconds(3f); 
			Destroy(gameObject);
		}
	}
	
	public void SetDead(bool Dead)
	{
		dead = Dead;
	}
}
