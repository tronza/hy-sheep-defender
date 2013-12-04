using System;
using UnityEngine;
using System.Collections;

public class SheepInsideFenceScript : MonoBehaviour
{
	public GameObject ground;
	private LostGUI lostGUI;
	
	public SheepInsideFenceScript ()
	{
	}
	
	// Use this for initialization
	void Start () {
		lostGUI = ground.GetComponent<LostGUI>();
		PlayerPrefs.SetInt(PlayerPrefKeys.LEVEL_GAMEOVER, 0);
	}
	
	void OnDestroy() {
		PlayerPrefs.SetInt(PlayerPrefKeys.LEVEL_GAMEOVER, 1);
		lostGUI.ShowGUI();
	}
}