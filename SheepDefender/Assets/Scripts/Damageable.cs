using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	
	/**
	 * dieEffect may be null. Then the Damageable object will just... disappear.
	 */
	public GameObject dieEffect;
	public float health = 100f;
	public int numberOfCoins = 5;
	public Transform coin_prefab;
	
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
			Destroy (gameObject);
			for (int i =0; i<numberOfCoins; i++) {
				if (gameObject.name.Contains ("Wolf")) {
					Instantiate (coin_prefab, new Vector3 (
					transform.localPosition.x+(0.5f*i),
					transform.localPosition.y + 0.5F,
					transform.localPosition.z+(0.25f*i)
				), Quaternion.identity);
				}
			}
			
			if (this.dieEffect) {
				Destroy (Instantiate (this.dieEffect, transform.position, Quaternion.identity), 1);	
			}
		}
	}

	public bool HasHealth ()
	{
		return this.health >= 0;
	}
}
