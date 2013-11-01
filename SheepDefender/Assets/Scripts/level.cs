using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class level : MonoBehaviour {
	public string levelName;//name of the level eg. level1
	public int levelDuration;//Shows the total time for completing the level
	public int timeEllapsed =0;//shows how much time has ellapsed
	private Dictionary<int, string[]> wolvesToSpawn = new Dictionary<int, string[]>();//Key is the point in time (second) in which wolves of the value (string array) are generated
	private GameObject[] spawnPoints;//The possible places the wolves can spawn to
	private int lastWolfsId =0;
	
	// Use this for initialization
	void Start () {
		char pathSeparator = Path.DirectorySeparatorChar;
		levelName = "level1";//to be changed when we have a level menu
		string levelPath = "levels" + pathSeparator + levelName +".ini";
		
		loadLevelFile(levelPath);//loads the level file
		
		spawnPoints = GameObject.FindGameObjectsWithTag("Spawn"); //all spawn points in scene
		
		InvokeRepeating("timerTick", 1, 1);
		
	}
	
	// Update is called once per frame
	void Update () {

	
	}
	
	private void timerTick(){
			timeEllapsed++;
				if(wolvesToSpawn.ContainsKey(timeEllapsed)){//There are wolves to spawn at this time point
					System.Random rnd = new System.Random();
					string[] wolves = wolvesToSpawn[timeEllapsed];
					for(int i=0;i<wolves.Length;i++){
						string wolfName = wolves[i];
						GameObject spawner = spawnPoints[rnd.Next(0, spawnPoints.Length)];
						spawnWolf(wolfName,lastWolfsId,spawner);
						lastWolfsId++;
					}
				}
	}
	
	private void spawnWolf(String prefabName, int wolfNumber, GameObject spawnPoint) {
		GameObject wolf = (GameObject)Instantiate(Resources.Load(prefabName), spawnPoint.transform.position, spawnPoint.transform.rotation);
		wolf.name = "enemyWolf"+wolfNumber; //unique name.
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
}
