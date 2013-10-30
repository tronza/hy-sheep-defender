using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public float health = 100f;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		if (!this.HasHealth ()) {
			Destroy (gameObject);
		}
	}

	public void ReceiveDamage (float damage)
	{
		// TODO: do something fancy with the amount of received damage ;-)
		// if (this.hasArmour ()) {
		// 	 this.health -= damage / 2;
		// } etc.

		this.health -= damage;
	}

	public bool HasHealth ()
	{
		return this.health >= 0;
	}
}
