using UnityEngine;
using System.Collections;

/**
 * Copyright 2014 Mika Hämäläinen, Kai Kulju
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
public class Winter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject terrain = GameObject.Find("Ground");
		Texture newOne = gameObject.GetComponent<GUITexture>().texture;
		terrain.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", newOne);
		if (GameObject.FindGameObjectWithTag("Night")==null){
			//change lights only if the night object is not in the scene
			GameObject light = GameObject.Find("Top Dir Light");
			light.GetComponent<Light>().intensity = 0.03f;
			GameObject dLight = GameObject.Find("Obq Dir Light");
			dLight.GetComponent<Light>().intensity = 0.37f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
