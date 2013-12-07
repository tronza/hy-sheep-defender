﻿using UnityEngine;
using System.Collections;

public class GameEnder : MonoBehaviour
{

	public LostGUI lostGUI;
	public CameraController cameraController;
	
	// Use this for initialization
	void Start ()
	{
		PlayerPrefs.SetInt (PlayerPrefKeys.LEVEL_GAMEOVER, 0);
	}
	
	void HealthZeroed ()
	{
		cameraController.ActivateMainCamera();
		PlayerPrefs.SetInt (PlayerPrefKeys.LEVEL_GAMEOVER, 1);
		lostGUI.ShowGUI ();
	}
}
