using UnityEngine;
using System.Collections;

public class LostGUI : MonoBehaviour {
	public GameObject terrain;
	private level levelClass;
	public GameObject endBackground;
	public GameObject endText;
	public GameObject retryText;
	public GameObject backToMenuText;
	
	private GameObject sheep;
	private GameObject guiObj;
	// Use this for initialization
	void Start () {
		levelClass = terrain.GetComponent<level>();
		guiObj = GameObject.Find("GUIobj");
		sheep = GameObject.Find("sheepPrefab");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowGUI(){
		//Show the end gui
		levelClass.enabled = false;
		guiObj.SetActive(false);
		if(sheep!=null){
			sheep.GetComponent<sheepScript>().enabled = false;
		}
		endBackground.GetComponent<GUITexture>().enabled = true;
		GUIText endMessage = endText.GetComponent<GUIText>();
		endMessage.text = "You lost ;-(";
		endMessage.enabled = true;
		retryText.GetComponent<GUIText>().enabled = true;
		backToMenuText.GetComponent<GUIText>().enabled = true;
		
	}
}
