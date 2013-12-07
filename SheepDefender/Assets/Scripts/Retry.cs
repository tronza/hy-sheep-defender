using UnityEngine;
using System.Collections;

public class Retry : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown(){
		PlayerPrefs.SetInt (PlayerPrefKeys.LEVEL_GAMEOVER, 0);
		Application.LoadLevel(Application.loadedLevel);
	}
}
