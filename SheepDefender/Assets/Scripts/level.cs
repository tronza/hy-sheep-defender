using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
//This class loads a level file and generates the wolves accordingly.
//The level file will be searched in a subfolder called "levels" in the game's root folder. E.g. levels/level_name.ini
//Wolf prefabs need to be in the Resources folder and have the exact same name that is used in the level file
//Wolves will spawn at random in the position of any gameobject that has been tagged with "Spawn" tag
public class level : MonoBehaviour {
	private string levelName;//name of the level eg. levels/level1.ini
	private int levelDuration;//Shows the total time for completing the level
	private int timeEllapsed =0;//shows how much time has ellapsed
	private Dictionary<int, string[]> wolvesToSpawn = new Dictionary<int, string[]>();//Key is the point in time (second) in which wolves of the value (string array) are generated
	private GameObject[] spawnPoints;//The possible places the wolves can spawn to
	private int lastWolfsId =0;
	
	public Texture progressbarBackground;
	public Texture progressbarForeground;
	
	public int progressBarX = 1;
	public int progressBarY = 1;
	public int progressBarH = 32;
	public int progressBarW = 256;
	
	// Use this for initialization
	void Start () {
		levelName = PlayerPrefs.GetString(PlayerPrefKeys.LEVEL_CURRENT);
		
		loadLevelFile(levelName );//loads the level file
		
		spawnPoints = GameObject.FindGameObjectsWithTag("Spawn"); //all spawn points in scene
		
		InvokeRepeating("timerTick", 1, 1);//Start the timer that checks for new wolves to spawn
		
	}
	
	private void drawProgressBar(){//draws the background
		GUI.DrawTexture(new Rect(progressBarX , progressBarY, progressBarW, progressBarH), progressbarBackground);
		float progress = ((float)timeEllapsed / (float)levelDuration);
		if(progress>1){//don't draw an overly long progressbar if the level has ended
			progress=1;
		}
		GUI.DrawTexture(new Rect(progressBarX , progressBarY, progressBarW * progress, progressBarH), progressbarForeground);
	}
	
	// Update is called once per frame
	void Update () {

	
	}
	void OnGUI() {
		drawProgressBar();//draws the progressbar to track the level's progress
		
	}
	
	private void timerTick(){//This occurs every second and checks for new wolves to be added
		timeEllapsed++;
		if(timeEllapsed>=levelDuration){//don't check for the wolves if the level has already ended
			
			//Place the end of a level function here
			
			return;
		}
		if(wolvesToSpawn.ContainsKey(timeEllapsed)){//There are wolves to spawn at this time point
			System.Random rnd = new System.Random();
			string[] wolves = wolvesToSpawn[timeEllapsed];
			List<int> spawnIndices = generateListWithNumbers(spawnPoints.Length);//This list contains the possible indicies so that if the number of wolves is smaller than the number of spawners, multiple wolves won't spawn on the same spot
			for(int i=0;i<wolves.Length;i++){
				if(spawnIndices.Count ==0 ){//If the indices list is empty, refill it (only incase there are more wolves at this time point in the level file than there are spawners)
					spawnIndices = generateListWithNumbers(spawnPoints.Length);
				}
				string wolfName = wolves[i];//The wolf to generate
				int randomIndex = rnd.Next(0, spawnIndices.Count);
				int spawnPosition = spawnIndices[randomIndex];
				spawnIndices.RemoveAt(randomIndex);//Remove the index from list, so that another wolf won't spawn there
				GameObject spawner = spawnPoints[spawnPosition];
				spawnGameObject(wolfName,lastWolfsId,spawner.transform.position, spawner.transform.rotation);
				lastWolfsId++;
			}
		}
	}
	
	private List<int> generateListWithNumbers(int maxInt){//Generates a list containing numbers from 0 to maxInt
		List<int> returnList = new List<int>();
		for(int i=0;i<maxInt;i++){
			returnList.Add(i);
		}
		return returnList;
	}
	
	private void spawnGameObject(String prefabName, int wolfNumber, Vector3 position, Quaternion rotation) {
		GameObject gameObj = (GameObject)Instantiate(Resources.Load(prefabName), position, rotation);
		if(wolfNumber!=-1){//-1 means that it's not a wolf
			gameObj.name = "enemyWolf"+wolfNumber; //unique name.
		}
	}
	
	private void loadLevelFile(string levelPath){//levelPath eg. levels/level1.ini
		IniFileTool iniReader = new IniFileTool(levelPath, true);
		levelDuration = iniReader.getValue("level","durationInSecs", 30);//loads the time
		
		//the following loop will load the times and wolves into the wolvesToSpawn dictionary
		int numberOfGroups = iniReader.getNumberOfGroups();
		for(int i=0; i<numberOfGroups; i++){
			string groupName = iniReader.getGroupByIndex(i);
			int sec = convertStringToInt(groupName);
			if(sec!=-1){//is a group that specifies time
				string wolves = iniReader.getValue(groupName,"spawn","wolf");
				wolvesToSpawn.Add(sec,wolves.Split(';')); //sets the time to the group name and wolves obtained from the level file to the dictionary
			}
		}
		
		//Loading defined sceneObjects
		int numberOfObjects = iniReader.getNumberOfKeys("sceneObjects");
		for(int i =0;i<numberOfObjects;i++){
			String newObj = iniReader.getValueByIndex("sceneObjects",i,null);
			if(newObj!=null){//there is a new object to add
				string[] objSettings = newObj.Split(';');
				string objName = objSettings[0];
				Vector3 position = new Vector3(convertStringToFloat(objSettings[1]),convertStringToFloat(objSettings[2]),convertStringToFloat(objSettings[3]));
				spawnGameObject(objName,-1,position,Quaternion.identity);
			}
		}
		
	}
	
	
	private int convertStringToInt(string stringToConvert){//Converts int to string, if fails, returns -1
		try
            {
                int number = Convert.ToInt32(stringToConvert);
                return number;
            }
            catch (Exception e) {
                return -1;
            }
	}
		private float convertStringToFloat(string stringToConvert){//Converts int to string, if fails, returns 0
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
