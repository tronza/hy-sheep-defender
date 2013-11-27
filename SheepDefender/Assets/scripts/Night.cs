using UnityEngine;
using System.Collections;

public class Night : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject light = GameObject.Find("Spotlight");
		light.GetComponent<Light>().intensity = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
