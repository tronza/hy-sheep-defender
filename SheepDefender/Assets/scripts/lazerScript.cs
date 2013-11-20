using UnityEngine;
using System.Collections;

public class lazerScript : MonoBehaviour {
	public float damage = 5f;
	public int speed = 16;
	public GameObject PlayerSheep;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(0,0,speed*Time.deltaTime);
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Attacker") {
			collision.gameObject.SendMessage("ReceiveDamage", damage);
			PlayerSheep = GameObject.Find ("Sheep");
			collision.gameObject.SendMessage ("ChangeTarget", PlayerSheep.transform);
		}
		
		Destroy (gameObject);
	}
}
