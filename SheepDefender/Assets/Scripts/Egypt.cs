using UnityEngine;
using System.Collections;

public class Egypt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject terrain = GameObject.Find("Ground");
		Texture newOne = gameObject.GetComponent<GUITexture>().texture;
		terrain.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", newOne);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
