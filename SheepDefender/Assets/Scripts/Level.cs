using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/**
 * Copyright 2014 Mika Hämäläinen, Agostino Sturaro
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
//This class loads a level file and generates the wolves accordingly.
//The level file will be searched in a subfolder called "levels" in the game's root folder. E.g. levels/level_name.ini
//Wolf prefabs need to be in the Resources folder and have the exact same name that is used in the level file
//Wolves will spawn at random in the position of any gameobject that has been tagged with "Spawn" tag
public class Level : MonoBehaviour {
	private string levelName;//name of the level eg. levels/level1.ini
	private int levelDuration;//Shows the total time for completing the level
	private int timeElapsed =0;//shows how much time has ellapsed
	private Dictionary<int, string[]> wolvesToSpawn = new Dictionary<int, string[]>();//Key is the point in time (second) in which wolves of the value (string array) are generated
	private Dictionary<int, string> messages = new Dictionary<int, string>();//Key is the point in time (second) in which the message (value) is shown
	private GameObject[] spawnPoints;//The possible places the wolves can spawn to
	private int lastWolfsId =0;
	
	public Texture progressbarBackground;
	public Texture progressbarForeground;
	public GameObject textObject;//The game object used to show messages
	private GUIText messageText;//The GUIText of the above game object
	
	private int progressBarX;
	public int progressBarY = 1;
	public int progressBarH = 32;
	public int progressBarW = 256;
	
	private VictoryGUI victoryGUI;
	
	private Dictionary<string, UnityEngine.Object> resourceCache;
	
	// Use this for initialization
	void Start () {
		resourceCache = new Dictionary<string, UnityEngine.Object>();
		
		progressBarX = Screen.width/2 - progressBarW/2;
		victoryGUI = gameObject.GetComponent<VictoryGUI>();
		
		levelName = PlayerPrefs.GetString(PlayerPrefKeys.LEVEL_CURRENT);
		LoadLevelFile(levelName );//loads the level file
		
		spawnPoints = GameObject.FindGameObjectsWithTag("Spawn"); //all spawn points in scene
		messageText = textObject.GetComponent<GUIText>();//Sets the gui text to modify		
		InvokeRepeating("TimerTick", 1, 1);//Start the timer that checks for new wolves to spawn
	}
	
	private void DrawProgressBar(){//draws the background
		GUI.DrawTexture(new Rect(progressBarX , progressBarY, progressBarW, progressBarH), progressbarBackground);
		float progress = ((float)timeElapsed / (float)levelDuration);
		if(progress>1){//don't draw an overly long progressbar if the level has ended
			progress=1;
		}
		GUI.DrawTexture(new Rect(progressBarX , progressBarY, progressBarW * progress, progressBarH), progressbarForeground);
	}
	
	UnityEngine.Object LoadResource(string path)
	{
		UnityEngine.Object resource;
		if(resourceCache.TryGetValue(path, out resource)) {
			return resource;
		} else {
			resource = Resources.Load(path);
			resourceCache.Add(path, resource);
			return resource;
		}
	}
	
	void OnGUI() {
		DrawProgressBar();//draws the progressbar to track the level's progress
		
	}
	
	private void TimerTick(){//This occurs every second and checks for new wolves to be added
		timeElapsed++;
		if(timeElapsed>=levelDuration){//don't check for the wolves if the level has already ended
			
			//End of the level
			GameObject wolf = GameObject.FindGameObjectWithTag("Attacker");
			if(wolf==null){//all wolves are dead!
				int score =  PlayerPrefs.GetInt (PlayerPrefKeys.SCORE);
				bool newHighScore = false;
				if(score > PlayerPrefs.GetInt(levelName,0)){
					PlayerPrefs.SetInt(levelName, score);//Sets the new high-score
					newHighScore = true;
				}
				PlayerPrefs.Save();
				victoryGUI.ShowGUI(score, newHighScore);
				CancelInvoke();//Disables this method
			}
			
			return;
		}
		if(wolvesToSpawn.ContainsKey(timeElapsed)){//There are wolves to spawn at this time point
			//Message
			messageText.text = messages[timeElapsed];
			
			//The wolves
			System.Random rnd = new System.Random();
			string[] wolves = wolvesToSpawn[timeElapsed];
			List<int> spawnIndices = GenerateListWithNumbers(spawnPoints.Length);//This list contains the possible indicies so that if the number of wolves is smaller than the number of spawners, multiple wolves won't spawn on the same spot
			for(int i=0;i<wolves.Length;i++){
				if(spawnIndices.Count ==0 ){//If the indices list is empty, refill it (only incase there are more wolves at this time point in the level file than there are spawners)
					spawnIndices = GenerateListWithNumbers(spawnPoints.Length);
				}
				string wolfName = wolves[i];//The wolf to generate
				int randomIndex = rnd.Next(0, spawnIndices.Count);
				int spawnPosition = spawnIndices[randomIndex];
				spawnIndices.RemoveAt(randomIndex);//Remove the index from list, so that another wolf won't spawn there
				GameObject spawner = spawnPoints[spawnPosition];
				SpawnGameObject(wolfName,lastWolfsId,spawner.transform.position, spawner.transform.rotation);
				lastWolfsId++;
			}
		}
	}
	
	private List<int> GenerateListWithNumbers(int maxInt){//Generates a list containing numbers from 0 to maxInt
		List<int> returnList = new List<int>();
		for(int i=0;i<maxInt;i++){
			returnList.Add(i);
		}
		return returnList;
	}
	
	private void SpawnGameObject(String prefabName, int wolfNumber, Vector3 position, Quaternion rotation) {
		GameObject gameObj = (GameObject)Instantiate(LoadResource(prefabName), position, rotation);
		if(wolfNumber!=-1){//-1 means that it's not a wolf
			gameObj.name = "enemyWolf"+wolfNumber; //unique name.
		}
	}
	
	private void LoadLevelFile(string levelPath){//levelPath eg. levels/level1.ini
		IniFileTool iniReader = new IniFileTool(levelPath, true);
		levelDuration = iniReader.getValue("level","durationInSecs", 30);//loads the time
		GameObject.Find("GameInfoObject").GetComponent<GameInfo>().levelName =iniReader.getValue("level","name", "Unnamed level");
		//the following loop will load the times and wolves into the wolvesToSpawn dictionary
		int numberOfGroups = iniReader.getNumberOfGroups();
		for(int i=0; i<numberOfGroups; i++){
			string groupName = iniReader.getGroupByIndex(i);
			int sec = ConvertStringToInt(groupName);
			if(sec!=-1){//is a group that specifies time
				string wolves = iniReader.getValue(groupName,"spawn","wolf");
				string message = iniReader.getValue(groupName,"message","");
				wolvesToSpawn.Add(sec,wolves.Split(';')); //sets the time as value and wolves obtained from the level file to the dictionary
				messages.Add(sec,message);//Adds a message to the list
			}
		}
		
		//Loading defined sceneObjects
		int numberOfObjects = iniReader.getNumberOfKeys("sceneObjects");
		for(int i =0;i<numberOfObjects;i++){
			String newObj = iniReader.getValueByIndex("sceneObjects",i,null);
			if(newObj!=null){//there is a new object to add
				string[] objSettings = newObj.Split(';');
				string objName = objSettings[0];
				Vector3 position = new Vector3(ConvertStringToFloat(objSettings[1]),ConvertStringToFloat(objSettings[2]),ConvertStringToFloat(objSettings[3]));
				SpawnGameObject(objName,-1,position,Quaternion.identity);
			}
		}
		
	}
	void OnDisable() {
		CancelInvoke();//Don't check for wolves if the script is disabled
	}
	
	private int ConvertStringToInt(string stringToConvert){//Converts int to string, if fails, returns -1
		try
            {
                int number = Convert.ToInt32(stringToConvert);
                return number;
            }
            catch (Exception e) {
                return -1;
            }
	}
		private float ConvertStringToFloat(string stringToConvert){//Converts int to string, if fails, returns 0
		try
            {
                float number = float.Parse(stringToConvert);
                return number;
            }
            catch (Exception e) {
                return 0;
            }
	}
}
