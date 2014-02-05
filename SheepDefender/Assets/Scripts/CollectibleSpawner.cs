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
public class CollectibleSpawner : MonoBehaviour {
	
	public Object prefab;
	public int numberToSpawn;
	
	public void HealthZeroed() {
		SpawnAtOnce();
	}
	
	int SpawnWithShift(Vector3 startPosition, Quaternion rotation, Vector3 shift, int times)
	{
		int spawned = 0;
		for(int i=0; i < times; ++i){
			//if a position is not free, just skip it
			if(true) { //TODO check if position free and not out of borders
				Instantiate(prefab, startPosition + shift * i, rotation);
				++spawned;
			}
		}
		return spawned;
	}
	
	// will work for prefabs of any size ;-)
	void SpawnAtOnce()
	{
		//arcane bit shift that's needed to get the "layer mask", used for raycasting
		int groundLayerMask = 1 << LayerMask.NameToLayer("GroundLayer");
		
		//cast ray from camera to ground, get intersection point with ground layer and move light there
		Ray rayDown = new Ray(transform.position, Vector3.down);
		Ray rayUp = new Ray(transform.position, Vector3.up);
		RaycastHit hitInfo;
		
		if (!Physics.Raycast (rayDown, out hitInfo, Mathf.Infinity, groundLayerMask)) {
			if (!Physics.Raycast (rayUp, out hitInfo, Mathf.Infinity, groundLayerMask)) {
				Debug.Log("No ground detected");
				return;
			}
		}
		
		Animation pAnimation = ((GameObject)prefab).GetComponentsInChildren<Animation>(true)[0];
		bool isAnimated= pAnimation != null;
		
		//this is to spawn at the right height from the ground, please do not use negative y
		Transform pTransform = ((GameObject)prefab).GetComponentsInChildren<Transform>(true)[0];
		Vector3 spawnAt = hitInfo.point;
		spawnAt.y += pTransform.position.y;
			
		Quaternion pRotation = pTransform.rotation;
		
		//get size of the box enclosing the prefab (MUST have a MeshFilter set for this to work)
		//NOTE: there is a bug in Unity that sometimes makes the MeshFilter component disappear
		Renderer pRenderer = ((GameObject)prefab).GetComponentsInChildren<Renderer>(true)[0];
		Vector3 boxSize = pRenderer.bounds.size;
		
		//keep spawning following in a counter-clockwise pattern
		int leftToSpawn = numberToSpawn;
		int round = 1;
		
		Vector3 leftShift = new Vector3(boxSize.x, 0F, 0F);
		Vector3 rightShift = new Vector3(-boxSize.x, 0F, 0F);
		Vector3 upShift = new Vector3(0F, 0F, boxSize.z);
		Vector3 downShift = new Vector3(0F, 0F, -boxSize.z);
		
		//if it is animated, we assume it can rotate on itself, and we need to account for that
		if (isAnimated) {
			float largest = Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
			leftShift.x = largest;
			rightShift.x = -largest;
			upShift.z = largest;
			downShift.z = -largest;
		}
		
		while(leftToSpawn > 0) {
			int doubleOfRound = round * 2;
			//spawn one in the center, then move 1 down
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, Vector3.zero, 1);
			spawnAt += downShift;
			
			//spawn going to the right
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, rightShift, Mathf.Min(leftToSpawn, doubleOfRound - 1));
			spawnAt += rightShift * (doubleOfRound - 1);
			
			//spawn going up
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, upShift, Mathf.Min(leftToSpawn, doubleOfRound));
			spawnAt += upShift * doubleOfRound;
			
			//spawn going to the left
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, leftShift, Mathf.Min(leftToSpawn, doubleOfRound));
			spawnAt += leftShift * doubleOfRound;
			
			//spawn going down
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, downShift, Mathf.Min(leftToSpawn, doubleOfRound));
			spawnAt += downShift * doubleOfRound;
			
			++round;
		}
	}
}
