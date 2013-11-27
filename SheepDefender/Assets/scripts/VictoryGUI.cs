using UnityEngine;
using System.Collections;

public class VictoryGUI : MonoBehaviour {
	
	public GameObject targetSheep;
	public GameObject guiObject;
	public GameObject endBackground;
	public GameObject endText;
	public GameObject backToMenuText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ShowGUI(){
		gameObject.GetComponent<level>().enabled = false;
		targetSheep.GetComponent<SheepInsideFenceScript>().enabled = false;
		guiObject.SetActive(false);
		//Show the end gui
		endBackground.GetComponent<GUITexture>().enabled = true;
		GUIText endMessage = endText.GetComponent<GUIText>();
		endMessage.text = "You win!";
		endMessage.enabled = true;
		backToMenuText.GetComponent<GUIText>().enabled = true;
		
	}
}
