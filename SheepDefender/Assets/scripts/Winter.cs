using UnityEngine;
using System.Collections;

public class Winter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject terrain = GameObject.Find("Ground");
		Texture newOne = gameObject.GetComponent<GUITexture>().texture;
		terrain.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", newOne);
		GameObject light = GameObject.Find("Spotlight");
		light.GetComponent<Light>().intensity = 0.03f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
