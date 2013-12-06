using UnityEngine;
using System.Collections;

public class Night : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//turn off the lights
		GameObject light = GameObject.Find("Top Dir Light");
		GameObject dLight = GameObject.Find("Obq Dir Light");
		GameObject fLight = GameObject.Find("Fill Dir Light");
		light.GetComponent<Light>().intensity = 0f;
		dLight.GetComponent<Light>().intensity = 0f;
		fLight.GetComponent<Light>().intensity = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
