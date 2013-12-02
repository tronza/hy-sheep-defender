using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour {
	public float damage;
	public float speed;
	public AmmoStorage.AmmoType ammoType; //must be set to the right type
	
	//this will preserve whatever the original rotation is and make it point the shooting direction
	public void Shoot(Vector3 direction) {
		//doing *= will not work (quaternions are tricky)
		transform.rotation = Quaternion.LookRotation(direction) * transform.rotation;
		rigidbody.velocity = direction*speed;
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.gameObject;
		Damageable damageable = other.GetComponent<Damageable>();
		
		//there is no neeed to send a costly message when you can get a reference
		if (damageable != null) {
			damageable.ReceiveDamage(damage);
		}
		Destroy(gameObject);
	}
}
