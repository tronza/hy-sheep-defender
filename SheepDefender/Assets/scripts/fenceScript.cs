using UnityEngine;
using System.Collections;

public class fenceScript : MonoBehaviour {
	
	int health = 100 ;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) {
		if( collision.gameObject.tag=="lazer" )
		{
			health-=25;
			if(health<=0)
			{
				
				Destroy(gameObject);
			}
		}
	}
}
