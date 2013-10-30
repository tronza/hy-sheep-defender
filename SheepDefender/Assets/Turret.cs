using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
	public float damage = 10f;
	public float cooldownTime = 2f; // Seconds
	public Damageable target = null; // TODO: change to private when not debugging!
	
	float cooldownRemaining = 0f;
	
	void Start ()
	{
	}

	void Update ()
	{
		// TODO: it takes one extra frame (!) to find a new target
		if (cooldownRemaining <= 0) {
			if (this.HasTargetAcquired ()) {
				DealDamage ();
				cooldownRemaining = cooldownTime;
			} else {
				this.AcquireNextTarget ();
			}
		}
		
		cooldownRemaining -= Time.deltaTime;
	}
	
	void DealDamage ()
	{
		if (this.target.HasHealth ()) {
			this.target.ReceiveDamage (damage);
			
			if (this.target.HasHealth ()) {
				this.AcquireNextTarget ();	
			}
			
		} else {
			this.target = null;
			
			this.AcquireNextTarget ();
		}
	}
	
	bool HasTargetAcquired ()
	{
		if (this.target != null) {
			return true;
		}
		
		return false;
	}
	
	void AcquireNextTarget ()
	{
		this.target = this.FindTarget ();
	}
	
	// TODO: implement something that checks for the nearest and most dangerous target
	Damageable FindTarget ()
	{
		return this.target;
	}
}
