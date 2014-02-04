using UnityEngine;
using System.Collections;

/**
 * Copyright 2014 Mika Hämäläinen, Agostino Sturaro, Kai Kulju
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
public class VictoryGUI : MonoBehaviour {
	
	public GameObject targetSheep;
	public GameObject guiObject;
	public GameObject endBackground;
	public GameObject endText;
	public GameObject backToMenuText;
	public string playerSheepName = "PlayerSheep";
	
	private GameObject playerSheep;
	
	// Use this for initialization
	void Start () {
		playerSheep = GameObject.Find(playerSheepName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ShowGUI(int score, bool newHighScore){
		gameObject.GetComponent<Level>().enabled = false;
		targetSheep.GetComponent<GameEnder>().enabled = false;
		playerSheep.GetComponent<Sheep>().enabled = false;
		guiObject.SetActive(false);
		
		string message = "You win!\nScore: " + score;
		if(newHighScore){
			message = message + "\nNEW HIGH SCORE!";
		}
		
		//Show the end gui
		endBackground.GetComponent<GUITexture>().enabled = true;
		GUIText endMessage = endText.GetComponent<GUIText>();
		endMessage.text = message;
		endMessage.enabled = true;
		backToMenuText.GetComponent<GUIText>().enabled = true;
		
	}
}
