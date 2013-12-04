using UnityEngine;
using System.Collections;
//This script is used in the main menu
public class ExitDialog : MonoBehaviour
{
	public string buttonType;
	public GameObject exitButton;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseDown ()
	{
		if (buttonType == "YES") {
			Application.Quit ();
		} else {
			exitButton.GetComponent<ExitButton> ().ShowDialog (false);
		}
	}
}
