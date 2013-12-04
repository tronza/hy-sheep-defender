using UnityEngine;
using System.Collections;
using Pathfinding;

public class WolfAI : MonoBehaviour
{
	// The AI's speed per second
	public float speed = 100;

	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
 
	// The tag of a hostile, who to chase down
	public string tagOfHostiles = "Defender";
	
	// Distance where you can do damage to a hostile
	public float attackDistance = 3;
	
	// How much health will an attack deal
	public float attackDamage = 5;
	private GameObject target;
	private Seeker seeker;
	private CharacterController controller;
	private Path path = null;
	private float nextAttack = 0;
	private float attackInterval = 2;
	private GameObject playerSheep;

	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	void Start ()
	{
		seeker = GetComponent<Seeker> ();
		controller = GetComponent<CharacterController> ();
		playerSheep = GameObject.Find ("PlayerSheep");
		
		// Start Repathing every 1-2 seconds
		InvokeRepeating ("Repath", 0, 1.0f + Random.Range (0.0f, 1.0f));
	}
	
	public virtual void Repath ()
	{
		// Check that there is a target somewhere and if not, try to find one
		if (target == null) {
			target = FindNearestHostile (tagOfHostiles);
		}
		
		// There is no target for now
		if (target == null) {
			animation.Play ("idle");	
		}
		
		// Nothing to do, just wait
		if (seeker == null || target == null || !seeker.IsDone ()) {
			return;
		}
		
		// Calculate a path from my current location to the target's location
		Path path = ABPath.Construct (transform.position, target.transform.position, null);
		
		// Start pathfinding
		seeker.StartPath (path, OnPathComplete);
	}
	
	public void FixedUpdate ()
	{
		// Attack if possible
		Attack ();

		// Try to find a path, otherwise just stay still
		if (FindPath ()) {
			Move ();
		} else {
			// This applies the physics to the gameobject even if it's not moving
			controller.SimpleMove (Vector3.zero);

			return;	
		}
	}
	
	public void Attack ()
	{
		if (target) {
			float distance = Vector3.Distance (target.transform.position, transform.position);
			
			// If the target is in the attacking distance, then start dealing damage!
			if (distance <= attackDistance) {
				if (nextAttack <= 0.0f) {
					animation.Play ("attack");
					target.SendMessage ("ReceiveDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
					
					nextAttack = attackInterval;
				}
				
				nextAttack -= Time.deltaTime; 
			}
		}
	}
	
	public bool FindPath ()
	{
		// Lets calculate a new path if there's none
		// i.e. if we had target in sight but it is lost now
		if (path == null) {
			Repath ();
			return false;
		}

		if (currentWaypoint >= path.vectorPath.Count) {
			Repath ();
			return false;
		}

		return true;
	}

	public void Move ()
	{
		Vector3 dir;
	
		//Direction to the next waypoint
		dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		
		// Start moving towards the direction
		controller.SimpleMove (dir);
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
		
		// Animation hack, so that the stupid wolf don't run sideways...
		dir.y = 0;
		transform.rotation = Quaternion.FromToRotation (Vector3.left, dir);
		
		animation.CrossFade ("run");
	}
	
	public GameObject FindNearestHostile (string tagOfHostiles)
	{
		GameObject[] hostiles; 
		float calcDist;
		GameObject closest = null;
		
		hostiles = GameObject.FindGameObjectsWithTag (tagOfHostiles);
		
		// If there are hostiles
		if (hostiles.Length > 0) {
			float minDist = Mathf.Infinity;
	
			// Check which one of the hostiles is the nearest one
			foreach (GameObject hostile in hostiles) {
				calcDist = Vector3.Distance (hostile.transform.position, transform.position);
				if (calcDist < minDist) {
					closest = hostile;
					minDist = calcDist;
				}
			}
		}
		
		// And return the closest hostile target
		return closest;
	}
	
	public void OnPathComplete (Path p)
	{
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	
	private bool CheckIfShotByPlayer (GameObject go)
	{
		// TODO: Add all "projectiles" shot by player here or make up some better solution
		
		return go.name.Contains ("lazerPrefab");
	}
	
	public void OnCollisionEnter (Collision collision)
	{
		// If wolf is being collided with something and player is not the target, lets proceed...
		if (target != playerSheep) {
			// Check if a colliding object is a projectile (we assume that only player shoots projectiles)
			if (CheckIfShotByPlayer (collision.gameObject)) {
				// Roar! Player is being targeted now
				target = playerSheep;
			}
		}
	}
}
