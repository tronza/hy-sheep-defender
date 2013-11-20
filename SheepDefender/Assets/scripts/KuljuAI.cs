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
	
    public void Start () {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
		
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        //path = seeker.StartPath (transform.position, target.transform.position, OnPathComplete);
    }
    
    public void OnPathComplete (Path p) {
		Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }
 
    public void FixedUpdate () {
		if (TargetInLineOfSight()) {
			// Lets forget about the path we had in mind before, because we have a target in sight
			path = null;
			currentWaypoint = 0;
			
			Vector3 dir = (target.transform.collider.ClosestPointOnBounds (transform.position) - transform.position).normalized;
			
			// Lets face to the direction of the target
			transform.forward = dir;
			
			dir *= speed * Time.fixedDeltaTime;
			controller.SimpleMove (dir);			
		} else {
			// Lets calculate a new path if there's none
			// i.e. if we had target in sight but it is lost now
			if (path == null) {
				path = seeker.StartPath (transform.position, target.transform.position, OnPathComplete);
	        }
	        
	        if (currentWaypoint >= path.vectorPath.Count) {
				path = seeker.StartPath (transform.position, target.transform.position, OnPathComplete);
				
				Debug.Log ("End Of Path Reached (" + path.vectorPath.Count + ") <= (" + currentWaypoint + ")");
	            return;
	        }
			
		  	//Direction to the next waypoint
	        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
	        dir *= speed * Time.fixedDeltaTime;
	        controller.SimpleMove (dir);
			
			transform.forward = dir; // Where the object looks towards
			
			//Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
	            return;
	        }
		}
		//cooldownRemaining -= Time.deltaTime;
    }
	
	bool TargetInLineOfSight()
	{
		RaycastHit hit;
		
		if (Physics.Raycast (transform.position, (target.transform.collider.ClosestPointOnBounds (transform.position) - transform.position), out hit)) {
			if (hit.collider.gameObject == target) {
				return true;	
			}
		}

		return false;
	}
}