using UnityEngine;
using System.Collections;

public class lazerScript : MonoBehaviour {
	
	int speed = 5;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(0,0,speed*Time.deltaTime);
		//Debug.
	
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag=="left" ||collision.gameObject.tag=="right" ||collision.gameObject.tag=="top" ||collision.gameObject.tag=="bottom" ||collision.gameObject.tag=="enemy" ||collision.gameObject.tag=="earth")
		{
			Destroy(gameObject);
		}
	}
}
