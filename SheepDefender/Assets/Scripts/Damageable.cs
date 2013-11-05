using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public GameObject dieEffect;
	public float health = 100f;
	
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
		
		if (!this.HasHealth ()) {
			// TODO: what happens to the referencing turrets etc after destroying the object?
			Destroy (gameObject, 2);
			gameObject.tag = null;
			
			Destroy (Instantiate (dieEffect, transform.position, Quaternion.identity), 3);
		}
	}

	public bool HasHealth ()
	{
		return this.health >= 0;
	}
}
