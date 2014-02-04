/**
 * Copyright 2014 Kai Kulju, Jannis Seemann, Agostino Sturaro, Mika Hämäläinen, Arthur Joly
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

public class Damageable : MonoBehaviour
{
	const string HEALTH_BAR = "HealthBar";
	/**
	 * dieEffect may be null. Then the Damageable object will just... disappear.
	 */
	public GameObject dieEffect;
	public float health = 100f;

	// You can override default from the Unity3D editor
	public GameObject healthBar;
	float originalScaleX;
	float originalHealth;
	
	// Use this for initialization
	void Start ()
	{
		// Instantiate the HealthBar prefab
		healthBar = (GameObject) Instantiate (
			(healthBar == null ? Resources.Load(HEALTH_BAR) : healthBar),
			transform.position + new Vector3(0.0f, 3.0f, 0.0f),
			Quaternion.identity
		);
		
		// Make the HealthBar follow the object the Damageable is attached to
		healthBar.transform.parent = transform;
		
		// Remember the initial size of the HealthBar and initial health amount
		originalScaleX = healthBar.transform.localScale.x;
		originalHealth = health;
		
		// Hide the HealthBar for now
		healthBar.SetActive(false);
	}

	// Update is called once per frame
	void Update ()
	{
		// Set the HealthBar to face the camera
		healthBar.transform.LookAt(Camera.main.transform.position, -Vector3.up);
	}

	public void ReceiveDamage (float damage)
	{	
		// Show the HealthBar because damage is received
		healthBar.SetActive(true);
		
		// TODO: do something fancy with the amount of received damage ;-)
		// if (this.hasArmour ()) {
		// 	 this.health -= damage / 2;
		// } etc.
		
		// Decrease the healthbar width relatively 
		healthBar.transform.localScale -= new Vector3(
			(originalScaleX / (originalHealth / damage)), 0.0f, 0.0f
		);
		
		// Decrease the health
		health -= damage;
		
		// If out of health
		if (!this.HasHealth ()) {
			// TODO: what happens to the referencing turrets etc after destroying the object?
			gameObject.SendMessage("HealthZeroed", SendMessageOptions.DontRequireReceiver);
			
			if (this.dieEffect) {
				Destroy (Instantiate (this.dieEffect, transform.position, Quaternion.identity), 1.0f);	
			}
			
			Destroy (gameObject);
			Destroy (healthBar);
		}
	}

	public bool HasHealth ()
	{
		return this.health > 0.0f;
	}
}
