using UnityEngine;
using System.Collections;

public class enemyWolfAI : MonoBehaviour {
	public Transform target, priorityTarget;
	private float nextAttack, attackInterval;

	public float attackDistance = 3;
	public float attackDamage = 5;
	
	GameObject collidedWith;
	
	void Awake() {
		SetTarget ();
	}
	
	// Use this for initialization
	void Start () {
		nextAttack = 0;
		attackInterval = 2;
		InvokeRepeating ("SetTarget", 0, 2);
	}

	// Update is called once per frame
	void Update () {
		if (priorityTarget != null) {
			target = priorityTarget; //on damage changes target
		}
		if (target != null) {
			//look at target
			//Vector3 dir = (target.position - transform.position).normalized;
			//dir.y = 0;
			//transform.rotation = Quaternion.FromToRotation(Vector3.left, dir);
			//Debug.DrawLine(target.position, transform.position, Color.red); //draws line to current target for viewing in editor

			//calculate time to attack
			float distance = Vector3.Distance(target.position, transform.position);
			if (distance <= attackDistance) {
				if (nextAttack > 0) {
					nextAttack -= Time.deltaTime; 
				}
				else if (nextAttack <= 0.0f) {
					Attack(target);
					nextAttack = attackInterval;
				}
			} //else {
			//	Move(target);
			//}

		} else if (target == null) {
			animation.CrossFade("idle");
		}
	}

	void SetTarget() {
		if (priorityTarget == null) {
			target = FindNearestObject();
		}
	}

	public Transform FindNearestObject() {
		GameObject[] sheep; 
		float calcDist;
		Transform closest = null;

		sheep = GameObject.FindGameObjectsWithTag("Defender"); //walls and sheep in array
		print ("number of sheep found: "+sheep.Length);
		
		if (sheep.Length > 0) {
			closest = sheep[0].transform;
			float minDist = Mathf.Infinity;
	
			foreach (GameObject s in sheep) { //for object in array, what is closest
				calcDist = Vector3.Distance(s.transform.position, transform.position);
				if (calcDist < minDist) {
					closest = s.transform;
					minDist = calcDist;
				}
			}
		}
		print ("chosen sheep "+closest.transform.name);
		return closest;
	}

	void Move(Transform target) {
		animation.CrossFade("run"); //run animation
		//avoid obstacles on terrain
		
		//this.gameObject.GetComponent<NavMeshAgent>().destination = target.position;
	}

	void Attack(Transform target) {
		float distance = Vector3.Distance(target.transform.position, transform.position);
		if (target != null) {
			if(distance <= attackDistance) {
				animation.Play("attack");
				target.SendMessage("ReceiveDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
			}
		} else {
			target = FindNearestObject(); //updates target to closest target
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		collidedWith=collision.gameObject;
	}
	
	void OnCollisionExit(Collision collision){
		collidedWith=null;
	}
	
	void ChangeTarget(Transform p_target) {
		priorityTarget = p_target;
	}
}