using UnityEngine;
using System.Collections;

/**
 * Copyright 2013-2014 Agostino Sturaro, Mika Hämäläinen, Kai Kulju
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
public class Collectible : MonoBehaviour {
	public float durationTime; //values <= 0 mean object does never disappear
	public Object vanishingEffect;
	public int storeValue;
	
	// Use this for initialization
	void Start ()
	{
		if (durationTime > 0F){
			//this is a non-blocking call
			StartCoroutine (destroyAfterTime (durationTime));
		}
	}
	
	//this is like a timer
	IEnumerator destroyAfterTime (float waitTime)
	{
		//wait for some seconds before proceeding to the next instruction
		yield return new WaitForSeconds(waitTime);
		//execution starts again here
		Destroy (Instantiate (vanishingEffect), 0.5F);
		Destroy (gameObject);
	}
}
