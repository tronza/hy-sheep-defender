using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	public Object shootablePrefab; //this is a prefab (an Object containing one or more GameObjects)
	public float rateOfFire;
	public int ammoLeft;
	
	//TODO: ammo count should be in sheep, weapon should only be loaded with a cartridge
	//all these can be implemented or not
//	public int cartridgeSize;
//	public int ammoInCartridge;
//	public float reloadTime;
	
	Shootable ammoLoaded;
	float readyToFire;
	Quaternion baseAmmoRotation;
	
	void Start() {
		//we know in this case the prefab is just 1 GameObject
		ammoLoaded = ((GameObject)shootablePrefab).GetComponentsInChildren<Shootable>(true)[0];
		readyToFire = Time.time;
		Transform baseAmmoTransform = ammoLoaded.GetComponentsInChildren<Transform>(true)[0];
		baseAmmoRotation = baseAmmoTransform.rotation;
	}
	
	void fire() {
		//make sure that the prefab orientation is preserved
		Quaternion ammoRotation = baseAmmoRotation * transform.rotation;
		Shootable shot = (Shootable)Instantiate(ammoLoaded, transform.position, ammoRotation);
		
		//shoot in the direction the weapon is facing
		shot.Shoot(Vector3.Normalize(transform.forward));
		--ammoLeft;
		//TODO: play sound
	}
	
	void PressTrigger() {
		if(Time.time > readyToFire && ammoLeft > 0){
			readyToFire = Time.time + rateOfFire;
			fire();
		}
	}
	
	//just a test
	void Update() {
		//TODO: this code should be in the sheep
		if (Input.GetMouseButton(0)) {
			PressTrigger();
		}
	}
}
