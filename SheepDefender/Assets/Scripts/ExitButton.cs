using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {
	public GameObject menuObject;
	public GameObject dialogBgr;
	public GameObject dialogText;
	public GameObject dialogYes;
	public GameObject dialogNo;
	public GameObject muteObject;
	private bool showDialog = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown ()
	{
			ShowDialog(!showDialog);
	}
	void OnGUI() {
		
	}
	public void ShowDialog(bool show){
		showDialog = show;
		menuObject.GetComponent<LevelMenu>().enabled = !show;
		muteObject.GetComponent<MuteButton>().enabled = !show;
		muteObject.GetComponent<GUITexture>().enabled = !show;
		gameObject.GetComponent<GUITexture>().enabled = !show;
		
		dialogBgr.GetComponent<GUITexture>().enabled = show;
		dialogText.GetComponent<GUIText>().enabled = show;
		dialogYes.GetComponent<GUIText>().enabled = show;
		dialogYes.GetComponent<ExitDialog>().enabled = show;
		dialogNo.GetComponent<GUIText>().enabled = show;
		dialogNo.GetComponent<ExitDialog>().enabled = show;
	}
}
