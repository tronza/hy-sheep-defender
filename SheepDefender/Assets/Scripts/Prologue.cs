using UnityEngine;
using System.Collections;

public class Prologue : MonoBehaviour {
	public GameObject prologueText;
	public GameObject prologueCam;
	public GameObject TitleCam;
	Vector3 endPoint = new Vector3(91.12849f,25.49816f,982.6024f);
	// Use this for initialization
	void Start () {
		TextMesh text = prologueText.GetComponent<TextMesh>();
		text.text ="In a farm not so far \naway from here, \na farmer had left his \nsheep alone in the field. \n\n Hungry wolves started to \ncome from the woods. \nTheir only goal was to \neat the meaty sheep\nthat were standing in\nthe field eating lawn. \n\nLuckily a space sheep\nhappened to crash his\nspace ship to the very\nsame field. He saw how\nmuch his primitive relatives\nneeded him and decided\nto help them. \n\nThe book of wise sheep\nknows him as \nthe Sheep Defender\n";
		InvokeRepeating ("MoveText", 0, 0.1f);//Starts the text's movement
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown){
			ShowTitle();
		}
	}
	
	void MoveText(){
		prologueText.transform.position = Vector3.MoveTowards (prologueText.transform.position, endPoint, 0.2f);
		if(prologueText.transform.position==endPoint){//The text has reached its end point
			ShowTitle();//Moves to the title screen
		}
	}
	
	void ShowTitle(){
		CancelInvoke();//MoveText won't be called again
		prologueCam.SetActive(false);
		TitleCam.GetComponent<Camera>().enabled=true;
		gameObject.GetComponent<TitleScreen>().enabled = true;
		gameObject.GetComponent<Prologue>().enabled = false;
	}
}
