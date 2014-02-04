/**
 * Copyright 2014 Agostino Sturaro, Mika Hämäläinen
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
﻿using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	
	public Object shootablePrefab; //this is a prefab (an Object containing one or more GameObjects)
	public float timeBetweenShots; //how much time to wait between shooting a round and the next
	
	Shootable loadedAmmo;
	AmmoStorage ammoStorage;
	Quaternion baseAmmoRotation;
	bool triggerPulled;
	float timeSinceShot;
	
	void Start ()
	{
		//we know in this case the prefab is just 1 GameObject
		loadedAmmo = ((GameObject)shootablePrefab).GetComponentsInChildren<Shootable> (true) [0];
		ammoStorage = AmmoStorage.Instance; //singleton
		Transform baseAmmoTransform = loadedAmmo.GetComponentsInChildren<Transform> (true) [0];
		baseAmmoRotation = baseAmmoTransform.rotation;
		triggerPulled = false;
		timeSinceShot = 0F;
	}
	
	void Shoot ()
	{
		//make sure that the prefab orientation is preserved
		Quaternion ammoRotation = baseAmmoRotation;
		Shootable shot = (Shootable)Instantiate (loadedAmmo, transform.position, ammoRotation);
		
		//shoot in the direction the weapon is facing
		shot.Shoot (Vector3.Normalize (transform.forward));
		ammoStorage.ConsumeAmmo (loadedAmmo.ammoType, 1);
		
		//play shooting sound
		if(PlayerPrefs.GetInt (PlayerPrefKeys.MUTE)!=1){
			audio.Play ();
		}
	}
	
	public void PullTrigger ()
	{
		if (!triggerPulled) {
			triggerPulled = true;
			timeSinceShot = 0F;
			Shoot ();
		} else {
			timeSinceShot += Time.deltaTime;
			if (timeSinceShot >= timeBetweenShots && ammoStorage.AvailabeAmmo (loadedAmmo.ammoType) > 0) {
				timeSinceShot = 0F;
				Shoot ();
			}
		}
	}
	
	public void ReleaseTrigger ()
	{
		triggerPulled = false;
		timeSinceShot = 0F;
	}
}
