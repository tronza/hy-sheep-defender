using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour {
	public float damage;
	public float speed;
	Vector3 movementDir;
	
	public void Shoot(Vector3 direction) {
		movementDir = direction;
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
