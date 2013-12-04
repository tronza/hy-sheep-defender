using UnityEngine;
using System.Collections;

public class VictoryGUI : MonoBehaviour {
	
	public GameObject targetSheep;
	public GameObject guiObject;
	public GameObject endBackground;
	public GameObject endText;
	public GameObject backToMenuText;
	
	private GameObject sheep;
	// Use this for initialization
	void Start () {
		sheep = GameObject.Find("sheepPrefab");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ShowGUI(){
		gameObject.GetComponent<Level>().enabled = false;
		targetSheep.GetComponent<SheepInsideFenceScript>().enabled = false;
		sheep.GetComponent<sheepScript>().enabled = false;
		guiObject.SetActive(false);
		//Show the end gui
		endBackground.GetComponent<GUITexture>().enabled = true;
		GUIText endMessage = endText.GetComponent<GUIText>();
		endMessage.text = "You win!";
		endMessage.enabled = true;
		backToMenuText.GetComponent<GUIText>().enabled = true;
		
	}
}
