using UnityEngine;
using System.Collections;

public class coinScript : MonoBehaviour {
	public GameObject dieEffect; //effect used when the coin disappear
	
	double destruction; //time when the coin has to disappear
	double timeRemaning = 5F; //time the coin stays in game
	
	/* Start() : is used for initialization
	 * set the destruction time
	 * */
	void Start () {
		destruction=Time.time + timeRemaning;
	}
	
	/* Update() : is called once per frame
	 * test the time until it has to destroy itself
	 * */
	void Update () {
		if(Time.time>=destruction)
		{
			Destroy(gameObject);
			Destroy (Instantiate (dieEffect, transform.position, Quaternion.identity), 0.5F);
		}
	}
	
	/* Collected() : is called by the sheep
	 * destroys itself
	 * */
	public void Collected()
	{
			Destroy(gameObject);
	}
}
