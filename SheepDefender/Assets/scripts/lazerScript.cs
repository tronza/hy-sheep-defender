using UnityEngine;
using System.Collections;

public class lazerScript : MonoBehaviour {
	
	int speed = 16;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(0,0,speed*Time.deltaTime);
		//Debug.
	
	}
	
	void OnCollisionEnter(Collision collision) {
		Debug.Log (gameObject);
		Destroy (gameObject);
	}
}
