using UnityEngine;
using System.Collections;

/**
 * Copyright 2013-2014 Agostino Sturaro, Kai Kulju
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
public class Trigger : MonoBehaviour {
	public string groundName = "Ground";
	public bool triggered = false;
	Projector projector;
	Color baseColor;
	
	void Start() {
		projector = GetComponent<Projector>();
		baseColor = projector.material.color;
	}
	
	void OnTriggerEnter(Collider other) {
		if (!string.Equals(other.name, groundName)) {
			triggered = true;
			projector.material.color = Color.red;
		}
    }
	
	void OnTriggerExit(Collider other) {
		if (!string.Equals(other.name, groundName)) {
	        triggered = false;
			projector.material.color = baseColor;
		}
    }
}
