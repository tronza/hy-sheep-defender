using UnityEngine;
using System.Collections;

public class SpawnPickables : MonoBehaviour
{
	public Object prefab;
	public int numberToSpawn;
	public float spawnInterval = 0.1F;
	
	// will work for prefabs of any size ;-)
	void Start ()
	{
//		Rigidbody pRigidBody = ((GameObject)prefab).GetComponentsInChildren<Rigidbody>(true)[0];
		
		//if there is physics, we throw stuff in the air and let physics do its trick
		//please keep the mass of 1
//		if (pRigidBody != null) {
//			StartCoroutine(spawnAtIntervals(spawnInterval));
//		}
		
		Animation pAnimation = ((GameObject)prefab).GetComponentsInChildren<Animation>(true)[0];
		SpawnAtOnce(pAnimation != null);
	}
	
	//this is like a timer
//	IEnumerator spawnAtIntervals (float interval)
//	{
//		GameObject spawned;
//		Vector3 forceVector;
//		for (int i = 0; i < numberToSpawn; ++i) {
//			spawned = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
//			
//			//add a force in a random direction
//			forceVector = Random.onUnitSphere;
//			forceVector.y = Mathf.Abs(forceVector.y);
//			spawned.rigidbody.AddForce(forceVector);
//			Debug.Log("Spawn dir " + forceVector);
//			
//			//wait before proceeding to the next iteration
//			yield return new WaitForSeconds(interval);
//		}
//		Destroy(this);
//	}
	
	int SpawnWithShift(Vector3 startPosition, Quaternion rotation, Vector3 shift, int times, string dir = "")
	{
		Debug.Log("" + times + " " + dir);
		
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
	
	void SpawnAtOnce(bool isAnimated)
	{
		//this is to spawn at the right height from the ground, please do not use negative y
		Transform pTransform = ((GameObject)prefab).GetComponentsInChildren<Transform>(true)[0];
		Vector3 spawnAt = transform.position;
		spawnAt.y += pTransform.position.y;
			
		Quaternion pRotation = pTransform.rotation;
		
		//get size of the box enclosing the prefab (MUST have a MeshFilter set for this to work)
		//NOTE: there is a bug in Unity that sometimes makes the MeshFilter component disappear
		Renderer pRenderer = ((GameObject)prefab).GetComponentsInChildren<Renderer>(true)[0];
		Vector3 boxSize = pRenderer.bounds.size;
		
		//keep spawning following in a counter-clockwise pattern
		int leftToSpawn = numberToSpawn;
		int round = 0;
		
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
			//spawn one in the center, then move 1 down
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, Vector3.zero, 1, "S");
			spawnAt += downShift;
			
			//spawn going to the right
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, rightShift, round * 2 - 1, "R");
			spawnAt += rightShift * (Mathf.Max(1, round * 2 - 1));
			
			//spawn going up
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, upShift, round * 2, "U");
			spawnAt += upShift * (Mathf.Max(1, round * 2));
			
			//spawn going to the left
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, leftShift, round * 2, "L");
			spawnAt += leftShift * (Mathf.Max(1, round * 2));
			
			//spawn going down
			leftToSpawn -= SpawnWithShift(spawnAt, pRotation, downShift, round * 2 + 1, "D");
			spawnAt += downShift * (Mathf.Max(1, round * 2));
			
			++round;
			Debug.Log("---");
		}
	}
}
