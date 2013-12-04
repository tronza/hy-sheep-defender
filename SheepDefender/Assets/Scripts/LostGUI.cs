using UnityEngine;
using System.Collections;

public class LostGUI : MonoBehaviour {
	public GameObject terrain;
	public GameObject endBackground;
	public GameObject endText;
	public GameObject retryText;
	public GameObject backToMenuText;
	
	private GameObject playerSheep;
	private GameObject guiObj;
	private Level levelComp;
	// Use this for initialization
	void Start () {
		levelComp = GetComponent<Level>();
		guiObj = GameObject.Find("GUIobj");
		playerSheep = GameObject.Find("PlayerSheep");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowGUI(){
		//Show the end gui
		levelComp.enabled = false;
		guiObj.SetActive(false);
		if(playerSheep!=null){
			playerSheep.GetComponent<Sheep>().enabled = false;
		}
		endBackground.GetComponent<GUITexture>().enabled = true;
		GUIText endMessage = endText.GetComponent<GUIText>();
		endMessage.text = "You lost ;-(";
		endMessage.enabled = true;
		retryText.GetComponent<GUIText>().enabled = true;
		backToMenuText.GetComponent<GUIText>().enabled = true;
		
	}
}
