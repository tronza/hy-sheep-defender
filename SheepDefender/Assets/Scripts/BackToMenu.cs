using UnityEngine;
using System.Collections;

public class BackToMenu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GoToMainMenu();
        
		}
	}

	void OnMouseDown ()
	{
		GoToMainMenu();
	}
	
	void GoToMainMenu(){
		PlayerPrefs.SetInt (PlayerPrefKeys.SKIP_TITLE, 1);
		Application.LoadLevel ("mainMenu");	
	}
}
