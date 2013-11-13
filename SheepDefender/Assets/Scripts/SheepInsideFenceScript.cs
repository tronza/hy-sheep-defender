using System;
using UnityEngine;
using System.Collections;

public class SheepInsideFenceScript : MonoBehaviour
{
	public SheepInsideFenceScript ()
	{
	}
	
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt(PlayerPrefKeys.LEVEL_GAMEOVER, 0);
	}
	
	void OnDestroy() {
		print ("Sheep has been destroyed.");
		PlayerPrefs.SetInt(PlayerPrefKeys.LEVEL_GAMEOVER, 1);
	}
}