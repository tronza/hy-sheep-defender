/**
 * Copyright 2014 Jannis Seemann, Mika Hämäläinen
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
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/**
 * The LevelMenu class. Used for displaying the level menu.
 * 
 * @since 0.1
 */
public class LevelMenu : MonoBehaviour {
	
	/**
	 * Helper class to encapsulate the ini file tool to allow easier unit testing later.
	 */
	public class LevelMenuItem {
		private string levelName;
		private string fileName;
		
		public LevelMenuItem(string levelName, string fileName) {
			this.fileName = fileName;
			this.levelName = levelName;
		}
		
		public string GetTitle() {
			return levelName;	
		}
		public string GetPath(){
			return fileName;
		}
	}
	
	/**
	 * Hellper class to encapsulate the rendering of the LevelMenu to allow easier unit testing later.
	 */
	public class LevelMenuRenderer {
		private LevelMenu levelMenu;
		private Boolean shown = false;
		
		public LevelMenuRenderer(LevelMenu levelMenu) {
			this.levelMenu = levelMenu;
		}
		
		public Boolean IsShown() {
			return shown;
		}
		
		public void Show() {
			this.shown = true;
		}
		
		public void OnGui() {			
			GUILayout.BeginScrollView(Vector2.zero, GUILayout.Width (300), GUILayout.Height (200));
			
			foreach(LevelMenu.LevelMenuItem levelMenuItem in this.levelMenu.GetItems()) {
				
				if (GUILayout.Button (levelMenuItem.GetTitle())) {
					this.levelMenu.LoadLevel(levelMenuItem);
				}
			}
			
			GUILayout.EndScrollView ();
			
		}
		
		public void Hide() {
			this.shown = false;
		}
	}
	
	/**
	 * The path in which the 
	 */
	private string iniPath = "levels";
	
	/**
	 * The items to be displayed in the menu
	 */
	private LinkedList<LevelMenuItem> items;
	
	/**
	 * The renderer for the menu. Might be NULL if the menu is not shown at all. 
	 * 
	 * @Nullable
	 */
	private LevelMenuRenderer levelMenuRenderer;
	
	public Texture testTexture;
	
	/**
	 * Constructor
	 */
	public void Start ()
	{
		this.items = new LinkedList<LevelMenuItem>();
		this.levelMenuRenderer = new LevelMenuRenderer(this);
		this.LoadLevelMenuItems ();
		this.Show ();
	}
	
	public void Update() {
		
	}
	
	public void OnGUI() {
		this.levelMenuRenderer.OnGui();
	}
	
	/**
	 * Parses the level directory. 
	 */
	private void LoadLevelMenuItems() {
		string[] files = Directory.GetFiles(this.iniPath);
		foreach(string fileName in files) {
			if(fileName.EndsWith(".ini")){
			IniFileTool iniFileTool = new IniFileTool(fileName);
			string levelName = iniFileTool.getValue("level","name","Unnamed level");
			int levelCompleted = PlayerPrefs.GetInt(fileName);
			if(levelCompleted!=0){//if the level has been completed, show it in the level name
					levelName = levelName + " (Score: "+levelCompleted+ ")";
				}
			this.items.AddLast(new LevelMenu.LevelMenuItem(levelName, fileName));
			}
		}
	}

	
	/**
	 * Show the menu to the screen.
	 */
	private void Show() {
		this.levelMenuRenderer.Show();
	}
	
	/**
	 * Hide the menu.
	 */
	private void Hide() {
		this.levelMenuRenderer.Hide();
	}
	
	public void LoadLevel(LevelMenuItem levelMenuItem) {
		this.Hide ();
		// @TODO( Load level.
		print ("loading level: " + levelMenuItem.GetPath());
		PlayerPrefs.SetString(PlayerPrefKeys.LEVEL_CURRENT, levelMenuItem.GetPath());
		Application.LoadLevel("BaseScene");
	}
	
	/**
	 * Get Level Menu items as list.
	 */
	protected LinkedList<LevelMenu.LevelMenuItem> GetItems() {
		return this.items;
	}
}

