using UnityEngine;
using System.Collections;

public class Night : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//turn off the lights
		GameObject light = GameObject.Find("Spotlight");
		GameObject dLight = GameObject.Find("Directional light");
		light.GetComponent<Light>().intensity = 0f;
		dLight.GetComponent<Light>().intensity = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
