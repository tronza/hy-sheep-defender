using UnityEngine;
using System.Collections;
using Pathfinding;

public class KuljuAI : MonoBehaviour {
	public GameObject target;
	private Seeker seeker;
    private CharacterController controller;
 
    //The calculated path
    public Path path = null;
    
    //The AI's speed per second
    public float speed = 100;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
	float repathRate = 0.5f;
	protected float lastPathSearch = -9999;
	
    public void Start () {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
		
		InvokeRepeating ("Repath", 0, 1.0f + Random.Range(0.0f, 1.0f));
    }
    
    public void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }
	
	public IEnumerator WaitToRepath () {
		float timeLeft = repathRate - (Time.time-lastPathSearch);
		
		yield return new WaitForSeconds (timeLeft);
		Repath ();
	}
	
	public virtual void Repath ()
	{
		if (target == null) {
			target = FindNearestObject();
		}
		
		lastPathSearch = Time.time;
		
		if (seeker == null || target == null || !seeker.IsDone ()) {
			StartCoroutine (WaitToRepath ());
			
			return;
		}
		
		Path path = ABPath.Construct(transform.position, target.transform.position, null);
		seeker.StartPath (path, OnPathComplete);
	}
	
	public void FixedUpdate () {
		// Add Repath () every 0.5f seconds
		
		// This applies the physics to the gameobject
		controller.SimpleMove(Vector3.zero);
		
		Vector3 dir;
	
		// Lets calculate a new path if there's none
		// i.e. if we had target in sight but it is lost now
		if (path == null) {
			Repath ();
			return;
		}
        
        if (currentWaypoint >= path.vectorPath.Count) {
			Repath ();
			return;
        }
		
	  	//Direction to the next waypoint
        dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        controller.SimpleMove (dir);
		
		//Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
		
		// Animation hack, so that the stupid wolf don't run sideways...
		dir.y = 0;
		transform.rotation = Quaternion.FromToRotation(Vector3.left, dir);
		
		animation.CrossFade("run");
    }
	
	public GameObject FindNearestObject() {
		GameObject[] sheep; 
		float calcDist;
		GameObject closest = null;

		sheep = GameObject.FindGameObjectsWithTag("Defender"); //walls and sheep in array
		
		if (sheep.Length > 0) {
			float minDist = Mathf.Infinity;
	
			foreach (GameObject s in sheep) { //for object in array, what is closest
				calcDist = Vector3.Distance(s.transform.position, transform.position);
				if (calcDist < minDist) {
					closest = s;
					minDist = calcDist;
				}
			}
		}
		
		return closest;
	}
}