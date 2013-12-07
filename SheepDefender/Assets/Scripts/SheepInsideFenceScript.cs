using System;
using UnityEngine;
using System.Collections;

public class SheepInsideFenceScript : MonoBehaviour
{
	public LostGUI lostGUI;

	void Start ()
	{
		PlayerPrefs.SetInt(PlayerPrefKeys.LEVEL_GAMEOVER, 0);
	}

	void GameOver ()
	{
		PlayerPrefs.SetInt (PlayerPrefKeys.LEVEL_GAMEOVER, 1);
		lostGUI.ShowGUI ();
	}

	void HealthZeroed ()
	{
		GameOver ();
	}

	void OnDisable () 
	{
		GameOver ();
	}
}