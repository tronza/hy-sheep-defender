using UnityEngine;
using System.Collections;

/**
 * Copyright 2014 Agostino Sturaro, Kai Kulju
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
public class Shootable : MonoBehaviour
{
	public AmmoStorage.AmmoType ammoType; //must be set to the right type
	public float damage;
	public float speed;
	public string friendlyObjTag = "Defender";
	
	//this will preserve whatever the original rotation is and make it point the shooting direction
	public void Shoot (Vector3 direction)
	{
		//doing *= will not work (quaternions are tricky)
		transform.rotation = Quaternion.LookRotation (direction) * transform.rotation;
		rigidbody.velocity = direction * speed;
	}
	
	void OnCollisionEnter (Collision collision)
	{
		GameObject other = collision.gameObject;
		Damageable damageable = other.GetComponent<Damageable> ();
		
		//there is no neeed to send a costly message when you can get a reference
		if (damageable != null) {
			if (other.tag != friendlyObjTag) {
				damageable.ReceiveDamage (damage);
			}
		}
		Destroy (gameObject);
	}
}
