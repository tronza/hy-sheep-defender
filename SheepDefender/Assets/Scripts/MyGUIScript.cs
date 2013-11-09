using UnityEngine;
using System.Collections;

public class MyGUIScript : MonoBehaviour
{
	//public vars
	public GameInfo myGameInfo;
	public Texture2D turretImage;
	public Texture2D wallImage;
	//NOTE: you could use a Transform reference, but this is clearer
	public Object turretPrefab;
	public Object wallPrefab;
		
	//private vars
	bool buttonsHidden = false;
	bool allHidden = false;
	GUIContent[] turrets;
	int selectedTurret = -1;
	bool selectedTurretChanged = false;
	bool placingTurret = false;
	GameObject createdTurret;
	bool discardClick = false;
	
	
	//TODO: find a way to scale button icon, without stretching it
	GUILayoutOption[] shopOptions = {GUILayout.MaxWidth (0.2f * Screen.width)};
	
	void Start ()
	{
		turrets = new GUIContent[] {
			new GUIContent ("Turret", turretImage),
			new GUIContent ("Wall", wallImage)
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
			GUILayout.Label ("Coins: " /*+ myGameInfo.coins*/);
			GUILayout.FlexibleSpace (); //fill
			GUILayout.Label ("Level: " /*+ myGameInfo.level*/);
			GUILayout.FlexibleSpace (); //fill
			GUILayout.Label ("Player name: " /*+ myGameInfo.playerName*/);
			GUILayout.EndHorizontal ();
			
			if (placingTurret) {
				if (createdTurret == null)
					GUILayout.Label ("PLACING MODE ACTIVE, left mouse to choose position");
				else
					GUILayout.Label ("PLACING MODE ACTIVE, right mouse to rotate, left mouse to confirm");
			}
			
			//display turret shop
			GUILayout.BeginVertical (shopOptions);
			
			//TODO: handle clicks on toggle
			buttonsHidden = GUILayout.Toggle (buttonsHidden, "hide");
			
			//NOTE: buttons cannot be grayed out in a SelectionGrid
			//-> do not use a SelectionGrid, use buttons in a layout
			
			if (!buttonsHidden) {
				if (GUILayout.Button (turrets [0])) {
					selectedTurretChanged = true;
					selectedTurret = 0;
				}
				if (GUILayout.Button (turrets [1])) {
					selectedTurretChanged = true;
					selectedTurret = 1;
				}
			}
			
			if (selectedTurretChanged) {
				Debug.Log ("Selected turret: " + selectedTurret);
				selectedTurretChanged = false;
				placingTurret = true;
				discardClick = true;
			} else if (discardClick) {
				discardClick = false;
			}
			
			GUILayout.FlexibleSpace (); //fill
			GUILayout.EndVertical ();
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		}
	}
	
	bool isAllHidden ()
	{
		return allHidden;
	}
	
	void setAllHidden (bool hidden)
	{
		this.allHidden = hidden;
	}
	
	//this is the placing of the turret
	void Update ()
	{
		//the first click is the one on the GUI and is thus discarded
		//TODO: use a "ghost" when positioning the turrets and walls, otherwise it can be placed over a wolf
		if (placingTurret) {
			if (createdTurret == null) {
				if (Input.GetMouseButtonDown (0) && !discardClick) {
					Plane plane = new Plane (Vector3.up, 0);
					float dist;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (plane.Raycast (ray, out dist)) {
						Vector3 point = ray.GetPoint (dist);
						createdTurret = CreateTurret (selectedTurret, point);
						DisableAllButRender (createdTurret);
					}
				}
			} else {
				if (Input.GetMouseButtonDown (1)) {
					//rotate turret 90 degrees a time
					createdTurret.transform.Rotate (0, 90, 0);
				}
				if (Input.GetMouseButtonDown (0)) {
					EnableAll (createdTurret);
					createdTurret = null;
					placingTurret = false;
				}
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Escape) && createdTurret == null) {
			placingTurret = false;
		}
	}
	
	//disable all components but the render
	void DisableAllButRender (GameObject obj)
	{
		//disable all scripts
		foreach (Behaviour childComponent in obj.GetComponentsInChildren<Behaviour>()) {
			childComponent.enabled = false;
		}
		obj.collider.enabled = false;
		//TODO: ensure every other component is disabled (e.g. what about rigidbody?)
	}
	
	void EnableAll (GameObject obj)
	{
		foreach (Behaviour childComponent in obj.GetComponentsInChildren<Behaviour>()) {
			childComponent.enabled = true;
		}
		obj.collider.enabled = true;
		//TODO: ensure every other component is enabled
	}
	
	GameObject CreateTurret (int turretKind, Vector3 position)
	{
		switch (turretKind) {
		case 0:
			//hardcoded adjustment for cube height
			Vector3 turretHeight = new Vector3 (0f, 0.5f, 0f);
			return (GameObject)Instantiate (turretPrefab, position + turretHeight, Quaternion.identity);
			break;
		case 1:
			//hardcoded adjustment for wall height
			Vector3 wallHeight = new Vector3 (0f, 2.5f, 0f);
			return (GameObject)Instantiate (wallPrefab, position + wallHeight, Quaternion.identity);
			break;
		default:
			Debug.Log ("Invalid turretKind value");
			return null;
			break;
		}
	}
}
