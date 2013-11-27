using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	public string groundName = "Ground";
	public bool triggered = false;
	Projector projector;
	Color baseColor;
	
	void Start() {
		projector = GetComponent<Projector>();
		baseColor = projector.material.color;
	}
	
	void OnTriggerEnter(Collider other) {
		if (!string.Equals(other.name, groundName)) {
			triggered = true;
			projector.material.color = Color.red;
		}
    }
	
	void OnTriggerExit(Collider other) {
		if (!string.Equals(other.name, groundName)) {
	        triggered = false;
			projector.material.color = baseColor;
		}
    }
}
