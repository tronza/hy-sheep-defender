using UnityEngine;
using System.Collections;

public class MyGUIScript : MonoBehaviour
{
	public GameInfo myGameInfo;
	public Texture2D turretImage;
	bool hideToggleValue = false;
//	public Texture[] turretImages;
//	int selectedTurret = -1;
	
	//TODO: find a way to scale button icon, without stretching it
//	GUILayoutOption[] hideButtonOptions = {GUILayout.ExpandHeight(true), GUILayout.MaxHeight (Screen.height)};
	GUILayoutOption[] shopOptions = {GUILayout.MaxWidth (0.2f * Screen.width)};
	
	void Start()
	{
		
	}
	
	void OnGUI ()
	{
		//use all the screen, dynamically adjust
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		
		//stack the different stuff
		GUILayout.BeginVertical ();
		
		//display player/level info
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace (); //fill
		
		//TODO: ask WTH player info they want to have
		//TODO: add some padding on the sides
		GUILayout.Label("Coins: "+myGameInfo.coins);
		GUILayout.FlexibleSpace (); //fill
		GUILayout.Label("Level: "+myGameInfo.level);
		GUILayout.FlexibleSpace (); //fill
		GUILayout.Label("Player name: "+myGameInfo.playerName);
		GUILayout.EndHorizontal ();
		
		//display turret shop and close button
//		GUILayout.BeginHorizontal (shopOptions);
		//TODO: handle button clicks
		
		//turret shop
		GUILayout.BeginVertical (shopOptions);
		GUILayout.Label("Shop");
		GUILayout.Toggle(hideToggleValue, "hide");
		
		//Buttons cannot be grayed out in a SelectionGrid -> discard
		selectedTurret= GUILayout.SelectionGrid(selectedTurret, turretImages, 1);
		//TODO: a way to reset selection (just set to -1)
		
		GUILayout.Button(new GUIContent("Turret 1", turretImage));
		GUILayout.Button(new GUIContent("Turret 2", turretImage));
		GUILayout.Button(new GUIContent("Turret 3", turretImage));
		GUILayout.FlexibleSpace (); //fill
		GUILayout.EndVertical ();
		
//		GUILayout.Button("<>", hideButtonOptions);
//		
//		GUILayout.EndHorizontal ();
		
//		GUILayout.FlexibleSpace (); //fill
		GUILayout.EndVertical ();
//		GUILayout.FlexibleSpace (); //fill
		GUILayout.EndArea ();
	}
}
