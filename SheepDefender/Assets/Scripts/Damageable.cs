using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	//public GameObject hitEffect;
	public GameObject dieEffect;
	
	public float health = 100f;
	bool isDead = false;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
	}

	public void ReceiveDamage (float damage)
	{
		// TODO: do something fancy with the amount of received damage ;-)
		// if (this.hasArmour ()) {
		// 	 this.health -= damage / 2;
		// } etc.

		this.health -= damage;
		
		//Instantiate (hitEffect, new Vector3(), new Quaternion());
		
		if (!this.HasHealth () && !this.isDead) {
			this.isDead = true;
			// TODO: what happens to the referencing turrets etc after destroying the object?
			Destroy (gameObject, 5);
			
			Destroy (Instantiate (dieEffect, transform.position, Quaternion.identity), 6);
		}
	}

	public bool HasHealth ()
	{
		return this.health >= 0;
	}
}
