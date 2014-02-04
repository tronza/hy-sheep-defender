/**
 * Copyright 2014 Mika Hämäläinen, Kai Kulju, Agostino Sturaro
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
﻿using UnityEngine;
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
