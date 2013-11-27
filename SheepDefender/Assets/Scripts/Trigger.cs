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
		Debug.Log(baseColor);
	}
	
	void OnTriggerEnter(Collider other) {
		Debug.Log("enter " + other.name + " " + string.Equals(other.name, groundName));
		if (!string.Equals(other.name, groundName)) {
			triggered = true;
			projector.material.color = Color.red;
		}
    }
	
	void OnTriggerExit(Collider other) {
		Debug.Log("exit " + other.name + " " + string.Equals(other.name, groundName));
		if (!string.Equals(other.name, groundName)) {
	        triggered = false;
			projector.material.color = baseColor;
		}
    }
}
