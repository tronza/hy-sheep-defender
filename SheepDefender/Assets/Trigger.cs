using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	public bool triggered = false;
	Projector projector;
	Color baseColor;
	
	void Start() {
		projector = GetComponent<Projector>();
		baseColor = projector.material.color;
	}
	
	void OnTriggerEnter(Collider other) {
		triggered = true;
		projector.material.color = Color.red;
    }
	
	void OnTriggerExit(Collider other) {
        triggered = false;
		projector.material.color = baseColor;
    }
}
