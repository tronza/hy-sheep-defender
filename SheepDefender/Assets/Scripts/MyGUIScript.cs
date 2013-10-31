using UnityEngine;
using System.Collections;

public class MyGUIScript : MonoBehaviour
{
	public GameInfo myGameInfo;
	public Texture2D turretImage;
	bool hideToggleValue = false;
	GUIContent[] turrets;
//	public Texture[] turretImages;
//	int selectedTurret = -1;
	
	//TODO: find a way to scale button icon, without stretching it
//	GUILayoutOption[] hideButtonOptions = {GUILayout.ExpandHeight(true), GUILayout.MaxHeight (Screen.height)};
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
		GUILayout.Toggle(hideToggleValue, "hide");
		
		//Buttons cannot be grayed out in a SelectionGrid -> discard
//		selectedTurret= GUILayout.SelectionGrid(selectedTurret, turretImages, 1);	

//		if(GUILayout.Button(new GUIContent("Turret 1", turretImage))) {
//			Debug.Log("turret 1");
//		}
//		if(GUILayout.Button(new GUIContent("Turret 2", turretImage))) {
//			Debug.Log("turret 2");
//		}
//		if(GUILayout.Button(new GUIContent("Turret 3", turretImage))) {
//			Debug.Log("turret 3");
//		}
		
		if(GUILayout.Button(turrets[0])) {
			Debug.Log("turret 0");
		}
		if(GUILayout.Button(turrets[1])) {
			Debug.Log("turret 1");
		}
		if(GUILayout.Button(turrets[2])) {
			Debug.Log("turret 2");
		}
		
		GUILayout.FlexibleSpace (); //fill
		GUILayout.EndVertical ();
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
}
