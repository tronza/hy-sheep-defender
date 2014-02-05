using UnityEngine;
using System.Collections;

/**
 * Copyright 2013-2014 Mika Hämäläinen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
			if (Input.GetKeyDown (KeyCode.Escape) && !showDialog) {
			ShowDialog(!showDialog);
		}
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
