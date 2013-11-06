using UnityEngine;
using System.Collections;

public class MyGUIScript : MonoBehaviour
{
	//public vars
	public GameInfo myGameInfo;
	public Texture2D turretImage;
	//NOTE: you could use a Transform reference, but this is clearer
	public Object turretPrefab;
		
	//private vars
	bool buttonsHidden = false;
	bool allHidden = false;
	GUIContent[] turrets;
	int selectedTurret = -1;
	bool selectedTurretChanged = false;
	bool placingTurret = false;
	bool discardClick = false;
	
	//TODO: find a way to scale button icon, without stretching it
	GUILayoutOption[] shopOptions = {GUILayout.MaxWidth (0.2f * Screen.width)};
	
	void Start()
	{
		turrets = new GUIContent[] {
			new GUIContent("Turret 1", turretImage),
			new GUIContent("Turret 2", turretImage),
			new GUIContent("Turret 3", turretImage)
		};
	}
	
	void OnGUI ()
	{
		if (!allHidden) {
			//use all the screen, dynamically adjust
			GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
			
			//stack the different stuff
			GUILayout.BeginVertical ();
			
			//display player/level info
			GUILayout.BeginHorizontal ();
			//TODO: ask WTH player info they want to have
			//TODO: add some padding on the sides
			GUILayout.Label("Coins: "+myGameInfo.coins);
			GUILayout.FlexibleSpace (); //fill
			GUILayout.Label("Level: "+myGameInfo.level);
			GUILayout.FlexibleSpace (); //fill
			GUILayout.Label("Player name: "+myGameInfo.playerName);
			GUILayout.EndHorizontal ();
			
			//display turret shop and close button
			GUILayout.BeginVertical (shopOptions);
			GUILayout.Label("Shop");
			//TODO: handle clicks on toggle
			buttonsHidden = GUILayout.Toggle(buttonsHidden, "hide");
			
			//NOTE: buttons cannot be grayed out in a SelectionGrid
			//-> do not use a SelectionGrid, use buttons in a layout
			
			if (!buttonsHidden) {
				if(GUILayout.Button(turrets[0])) {
					selectedTurretChanged = true;
					selectedTurret = 0;
				}
				if(GUILayout.Button(turrets[1])) {
					selectedTurretChanged = true;
					selectedTurret = 1;
				}
				if(GUILayout.Button(turrets[2])) {
					selectedTurretChanged = true;
					selectedTurret = 2;
				}
			}
			
			if (selectedTurretChanged) {
				Debug.Log("Selected turret: " + selectedTurret);
				selectedTurretChanged = false;
				placingTurret = true;
				discardClick = true;
			}
			else if (discardClick) {
				discardClick = false;
			}
			
			GUILayout.FlexibleSpace (); //fill
			GUILayout.EndVertical ();
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		}
	}
	
	bool isAllHidden()
	{
		return allHidden;
	}
	
	void setAllHidden(bool hidden) {
		this.allHidden = hidden;
	}
	
	//-1 means none
	int GetSelectedTurret()
	{
		return selectedTurret;
	}
	
	//this is the placing of the turret
	void Update()
	{
		//the first click is on the button and should be discarded
		if(placingTurret && Input.GetMouseButtonDown(0) && !discardClick) {
			Plane plane = new Plane (Vector3.up, 0);
			float dist;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (plane.Raycast (ray, out dist)) {
				Vector3 point = ray.GetPoint (dist);
				//hardcoded adjustment for cube height
				Vector3 vectorHalfUp = new Vector3 (0f, 0.5f, 0f);
				Instantiate (turretPrefab, point + vectorHalfUp, Quaternion.identity);
				placingTurret = false;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			placingTurret = false;
		}
	}
}
