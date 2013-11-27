using UnityEngine;
using System.Collections;

public class LostGUI : MonoBehaviour {
	public GameObject terrain;
	private level levelClass;
	public GameObject endBackground;
	public GameObject endText;
	public GameObject retryText;
	public GameObject backToMenuText;
	// Use this for initialization
	void Start () {
		levelClass = terrain.GetComponent<level>();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowGUI(){
		//Show the end gui
		levelClass.enabled = false;
		endBackground.GetComponent<GUITexture>().enabled = true;
		GUIText endMessage = endText.GetComponent<GUIText>();
		endMessage.text = "You lost ;-(";
		endMessage.enabled = true;
		retryText.GetComponent<GUIText>().enabled = true;
		backToMenuText.GetComponent<GUIText>().enabled = true;
		
	}
}
