using UnityEngine;
using System.Collections;

public class Winter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject terrain = GameObject.Find("Ground");
		Texture newOne = gameObject.GetComponent<GUITexture>().texture;
		terrain.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", newOne);
		if (GameObject.FindGameObjectWithTag("Night")==null){
			//change lights only if the night object is not in the scene
			GameObject light = GameObject.Find("Top Dir Light");
			light.GetComponent<Light>().intensity = 0.03f;
			GameObject dLight = GameObject.Find("Obq Dir Light");
			dLight.GetComponent<Light>().intensity = 0.37f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
