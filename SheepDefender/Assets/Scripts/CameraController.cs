/**
 * Copyright 2014 Kai Kulju, Mika Hämäläinen, Agostino Sturaro
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

public class CameraController : MonoBehaviour
{
	public GameObject mainCamera;
	public GameObject thirdPersonCamera;
	public GameObject playerSheep;
	
	// Use this for initialization
	void Start ()
	{
		ActivateThirdPersonCamera ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("c")) {
			ChangeCamera ();
		}
	}
	
	public void ActivateMainCamera ()
	{
		if (playerSheep != null) {
			playerSheep.GetComponent<Sheep> ().SetMovementMode (Sheep.MovementMode.Stopped);
		}
		if (thirdPersonCamera != null) {
			thirdPersonCamera.GetComponent<Camera> ().enabled = false;
			mainCamera.GetComponent<Camera> ().enabled = true;
		}
	}
	
	public void ActivateThirdPersonCamera ()
	{
		if (thirdPersonCamera != null) {
			mainCamera.GetComponent<Camera> ().enabled = false;
			thirdPersonCamera.GetComponent<Camera> ().enabled = true;
		}
		if (playerSheep != null) {
			playerSheep.GetComponent<Sheep> ().SetMovementMode (Sheep.MovementMode.DeltaMouse);
		}
	}
	
	public void ChangeCamera ()
	{
		if (mainCamera.GetComponent<Camera> ().enabled) {
			ActivateThirdPersonCamera ();
		} else {
			ActivateMainCamera ();
		}
	}
}
