using UnityEngine;
using System.Collections;

public class coinScript : MonoBehaviour {
	public GameObject dieEffect;
	
	double destruction;
	double timeRemaning = 3F;
	
	// Use this for initialization
	void Start () {
		destruction=Time.time + timeRemaning;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time>=destruction)
		{
			this.End();
		}
	}
	
	public void End()
	{
		Destroy(gameObject);
		Destroy (Instantiate (dieEffect, transform.position, Quaternion.identity), 0.5F);
	}
}
