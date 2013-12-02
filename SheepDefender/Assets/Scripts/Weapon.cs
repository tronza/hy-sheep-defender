using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	public Object shootablePrefab; //this is a prefab (an Object containing one or more GameObjects)
	public float timeBetweenShots; //how much time to wait between shooting a round and the next
	
	Shootable loadedAmmo;
	AmmoStorage ammoStorage;
	float readyToFire;
	Quaternion baseAmmoRotation;
	bool triggerPulled;
	float timeSinceShot;
	
	void Start() {
		//we know in this case the prefab is just 1 GameObject
		loadedAmmo = ((GameObject)shootablePrefab).GetComponentsInChildren<Shootable>(true)[0];
		ammoStorage = AmmoStorage.Instance; //singleton
		readyToFire = Time.time;
		Transform baseAmmoTransform = loadedAmmo.GetComponentsInChildren<Transform>(true)[0];
		baseAmmoRotation = baseAmmoTransform.rotation;
		triggerPulled = false;
		timeSinceShot = 0F;
	}
	
	void Shoot() {
		//make sure that the prefab orientation is preserved
		Quaternion ammoRotation = baseAmmoRotation * transform.rotation;
		Shootable shot = (Shootable)Instantiate(loadedAmmo, transform.position, ammoRotation);
		
		//shoot in the direction the weapon is facing
		shot.Shoot(Vector3.Normalize(transform.forward));
		ammoStorage.ConsumeAmmo(loadedAmmo.ammoType, 1);
		
		//play shooting sound
		audio.Play();
	}
	
	void PullTrigger() {
		if(!triggerPulled) {
			triggerPulled = true;
			timeSinceShot = 0F;
			Shoot();
		} else {
			timeSinceShot += Time.deltaTime;
			if(timeSinceShot >= timeBetweenShots && ammoStorage.AvailabeAmmo(loadedAmmo.ammoType) > 0) {
				timeSinceShot = 0F;
				Shoot();
			}
		}
	}
	
	void ReleaseTrigger() {
		triggerPulled = false;
		timeSinceShot = 0F;
	}
	
	//just a test
	void Update() {
		//TODO: this code should be in the sheep
		if (Input.GetButton("Fire1")) {
			PullTrigger();
		} else if (Input.GetButtonUp("Fire1")) {
			ReleaseTrigger();
		}
	}
}
