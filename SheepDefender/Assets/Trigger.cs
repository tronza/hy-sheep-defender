using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	public bool triggered = false;
	Projector projector;
	
	void Start() {
		projector = GetComponent<Projector>();
		projector.material.color = Color.white;
	}
	
	void OnTriggerEnter(Collider other) {
		triggered = true;
		projector.material.color = Color.red;
    }
	
	void OnTriggerExit(Collider other) {
        triggered = false;
		projector.material.color = Color.white;
    }
}
