using System;
using UnityEngine;
using System.Collections;

public class SheepInsideFenceScript : MonoBehaviour
{
	public LostGUI lostGUI;
	
	public SheepInsideFenceScript ()
	{
	}
	
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt(PlayerPrefKeys.LEVEL_GAMEOVER, 0);
	}
	
	void Destroyed() {
		PlayerPrefs.SetInt(PlayerPrefKeys.LEVEL_GAMEOVER, 1);
		lostGUI.ShowGUI();
	}
	void OnDisable(){
		Destroyed();
	}
}