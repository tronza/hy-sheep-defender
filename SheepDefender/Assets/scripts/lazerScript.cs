using UnityEngine;
using System.Collections;

public class lazerScript : MonoBehaviour {
	public float damage = 5f;
	public int speed = 16;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(0,0,speed*Time.deltaTime);
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Attacker") {
			collision.gameObject.SendMessage("ReceiveDamage", 5f);
		}
		
		Destroy (gameObject);
	}
}
