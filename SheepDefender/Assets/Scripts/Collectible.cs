using UnityEngine;
using System.Collections;

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
