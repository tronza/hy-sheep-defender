using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	void Start () {
		AstarPath.active.UpdateGraphs(gameObject.collider.bounds);
	}
	
	void Update () {
	}
	
	void OnDisable () {
		// This will be called also at the end of the game and AstarPath is not active anymore
		if (AstarPath.active) {
			Bounds oldBounds = gameObject.collider.bounds;
			
			AstarPath.active.UpdateGraphs(oldBounds, 1.0f);
		}
	}
}
