using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	public Object shootablePrefab; //this is a prefab (an Object containing one or more GameObjects)
	public float timeBetweenShots; //how much time to wait between shooting a round and the next
	public int ammoLeft;
	
	//TODO: ammo count should be in sheep, weapon should only be loaded with a cartridge
	//all these can be implemented or not
//	public int cartridgeSize;
//	public int ammoInCartridge;
//	public float reloadTime;
	
	Shootable ammoLoaded;
	AudioSource shootAudioSrc;
	float readyToFire;
	Quaternion baseAmmoRotation;
	bool triggerPulled;
	float timeSinceShot;
	
	void Start() {
		//we know in this case the prefab is just 1 GameObject
		ammoLoaded = ((GameObject)shootablePrefab).GetComponentsInChildren<Shootable>(true)[0];
		shootAudioSrc = GetComponent<AudioSource>();
		readyToFire = Time.time;
		Transform baseAmmoTransform = ammoLoaded.GetComponentsInChildren<Transform>(true)[0];
		baseAmmoRotation = baseAmmoTransform.rotation;
		triggerPulled = false;
		timeSinceShot = 0F;
	}
	
	void Shoot() {
		//make sure that the prefab orientation is preserved
		Quaternion ammoRotation = baseAmmoRotation * transform.rotation;
		Shootable shot = (Shootable)Instantiate(ammoLoaded, transform.position, ammoRotation);
		
		//shoot in the direction the weapon is facing
		shot.Shoot(Vector3.Normalize(transform.forward));
		--ammoLeft;
		
		//play shooting sound
		shootAudioSrc.Play();
	}
	
	void PullTrigger() {
		if(!triggerPulled) {
			Debug.Log("pulling trigger 1st time");
			triggerPulled = true;
			timeSinceShot = 0F;
			Shoot();
		} else {
			timeSinceShot += Time.deltaTime;
			Debug.Log("timeSinceShot " + timeSinceShot);
			if(timeSinceShot >= timeBetweenShots && ammoLeft > 0) {
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
