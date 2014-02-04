using UnityEngine;
using System.Collections;

/**
 * Copyright 2014 Mika Hämäläinen
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
public class TitleScreen : MonoBehaviour
{
	public GameObject effectFlare;
	public GameObject pressKeyText;
	public GameObject gameLogo;
	public GameObject muteButton;
	public GameObject exitButton;
	Vector3 endPointForFlare = new Vector3 (39.67602f, 9.015059f, 591.6068f);//The end point of the flare's movement
	bool flareReturning = false;//used to store the direction towards which the flare is going.
	// Use this for initialization
	void Start ()
	{
		InvokeRepeating ("MoveFlare", 0, 0.1f);//Starts the flare's movement
		if(PlayerPrefs.GetInt(PlayerPrefKeys.SKIP_TITLE)==1){
			PlayerPrefs.SetInt(PlayerPrefKeys.SKIP_TITLE, 0);
			ShowLevelMenu();//if skip is set
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.anyKeyDown) {//Close the title and show the level menu
			ShowLevelMenu();
		}
	}
	
	void ShowLevelMenu(){
			pressKeyText.SetActive(false);
			gameLogo.SetActive(false);
			effectFlare.SetActive(false);
			CancelInvoke();//MoveFlare won't be called again
			gameObject.GetComponent<LevelMenu>().enabled = true;
			muteButton.GetComponent<GUITexture>().enabled = true;
			exitButton.GetComponent<GUITexture>().enabled = true;
			muteButton.GetComponent<MuteButton>().enabled = true;
			exitButton.GetComponent<ExitButton>().enabled = true;
			gameObject.GetComponent<TitleScreen>().enabled = false;
	}
	
	void MoveFlare ()//Moves the flare from one direction to another
	{
		effectFlare.transform.position = Vector3.MoveTowards (effectFlare.transform.position, endPointForFlare, 0.2f);
		if (effectFlare.transform.position == endPointForFlare) {
			if (flareReturning) {
				endPointForFlare = new Vector3 (39.67602f, 9.015059f, 591.6068f);
				flareReturning = false;
			} else {
				endPointForFlare = new Vector3 (39.67602f, 9.015059f, 615.2715f);
				flareReturning = true;
			}
		}
	}
}
