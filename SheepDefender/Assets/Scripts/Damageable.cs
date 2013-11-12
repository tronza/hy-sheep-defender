using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public GameObject dieEffect;
	public float health = 100f;
	
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
			
			Destroy (Instantiate (dieEffect, transform.position, Quaternion.identity), 1);
			if(gameObject.name=="Wolf"){
				Instantiate(coin_prefab, new Vector3(gameObject.transform.localPosition.x,gameObject.transform.localPosition.y+0.5F,gameObject.transform.localPosition.z), new Quaternion(Random.Range(0.0F, 180.0F),Random.Range(0.0F, 180.0F),Random.Range(0.0F, 180.0F),0));
			}
			
		}
	}

	public bool HasHealth ()
	{
		return this.health >= 0;
	}
}
