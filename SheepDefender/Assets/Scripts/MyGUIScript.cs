using UnityEngine;
using System.Collections;

public class MyGUIScript : MonoBehaviour
{
	//public vars
	public GameInfo myGameInfo;
	public Object[] placeablePrefabs;
	public Collider ground;
	public GameObject lightObj;
	
	//private vars
	Projector lightProj;
	bool buttonsHidden = false;
	bool allHidden = false;
	GUIContent[] turrets;
	int selectedTurret = -1;
	bool selectedTurretChanged = false;
	bool placingTurret = false;
	GameObject createdTurret;
	bool discardClick = false;
	bool startedPlacing = false;
	
	int lastUpdateFrame;
	int lastOnGUIFrame;
	int OnGUICallsThisFrame;
	
	
	//TODO: find a way to scale button icon, without stretching it
	GUILayoutOption[] shopOptions = {GUILayout.MaxWidth (0.2f * Screen.width)};
	
	void Start ()
	{
		turrets = new GUIContent[placeablePrefabs.Length];
		
		//foreach (Object obj in placeablePrefabs)
		for (int i = 0; i < placeablePrefabs.Length; ++i)
		{
			Object prefab = placeablePrefabs[i];
			if(prefab is GameObject){
				Placeable placeable = ((GameObject)prefab).GetComponentsInChildren<Placeable>(true)[0];
				string pName = placeable.name;
				Texture2D pIcon = placeable.icon;
				turrets[i] = new GUIContent (pName, pIcon);
			}
		}
		
		//adjust rotation (since aspectRatio resizes on another axis)
		rotateObjOnY(lightObj, 90f);
		lightProj = lightObj.GetComponent<Projector>();
		lightProj.orthographic = true;
		lightProj.enabled = false;
	}
	
	void OnGUI ()
	{
		//TODO: use this synchronization to assign variables only once a frame
		lastOnGUIFrame=Time.frameCount;
		
		if (lastOnGUIFrame > lastUpdateFrame) {
			++OnGUICallsThisFrame;
		} else {
			OnGUICallsThisFrame = 1;
		}
		
		//if (!allHidden && PlayerPrefs.GetInt (PlayerPrefKeys.LEVEL_GAMEOVER) == 0) {
		if (!allHidden) {
			//use all the screen, dynamically adjust
			GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
			
			//stack the different stuff
			GUILayout.BeginVertical ();
			
			//display player/level info
			GUILayout.BeginHorizontal ();
			//TODO: ask WTH player info they want to have
			//TODO: add some padding on the sides
			GUILayout.Label ("Coins: " + myGameInfo.coins);
			GUILayout.FlexibleSpace (); //fill
			GUILayout.Label ("Level: " + myGameInfo.level);
			GUILayout.FlexibleSpace (); //fill
			GUILayout.Label ("Player name: " + myGameInfo.playerName);
			GUILayout.EndHorizontal ();
			
			if (placingTurret) {
				GUILayout.Label ("PLACING MODE ACTIVE, left click to choose position, right click to rotate");
			}
			
			//display turret shop
			GUILayout.BeginVertical (shopOptions);
			
			//TODO: handle clicks on toggle
			buttonsHidden = GUILayout.Toggle (buttonsHidden, "hide");
			
			//NOTE: buttons cannot be grayed out in a SelectionGrid
			//-> do not use a SelectionGrid, use buttons in a layout
			
			if (!buttonsHidden) {
				for (int i = 0; i < placeablePrefabs.Length; ++i) {
					if (GUILayout.Button (turrets [i])) {
						selectedTurretChanged = true;
						selectedTurret = i;
					}
				}
			}
			
			if (selectedTurretChanged) {
				Debug.Log ("Selected turret: " + selectedTurret);
				selectedTurretChanged = false;
				placingTurret = true;
				discardClick = true;
				startedPlacing = true;
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
	
	//changed this so many times that it's better as a function
	void rotateObjOnY(GameObject obj, float degrees) {
		obj.transform.RotateAround(obj.transform.position, Vector3.up, degrees);
//		Debug.Log(obj.transform.rotation.eulerAngles);
	}
	
	//this is the placing of the turret
	void Update ()
	{
		lastUpdateFrame=Time.frameCount;
		
		if (lastUpdateFrame > lastOnGUIFrame) {
			OnGUICallsThisFrame = 0;
		}
		
		//the first click is the one on the GUI and is thus discarded
		//TODO: use a "ghost" when positioning the turrets and walls, otherwise it can be placed over a wolf
		if (placingTurret) {
			if (startedPlacing) {
				Object prefab = placeablePrefabs[selectedTurret];
				
				//TODO: disable buttons when there are not enough money
				Placeable placeable = ((GameObject)prefab).GetComponentsInChildren<Placeable>(true)[0];
				if(myGameInfo.coins < placeable.price) {
					Debug.Log("Not enough money");
					placingTurret = false;
					startedPlacing = false;
					return;
				}
				myGameInfo.coins -= placeable.price;
				lightProj.enabled = true;
				
				//get size of the box enclosing the prefab (MUST have a MeshFilter set for this to work)
				//NOTE: there is a bug in Unity that sometimes makes the MeshFilter component disappear
				Renderer pRenderer = ((GameObject)prefab).GetComponentsInChildren<Renderer>(true)[0];
				Vector3 boxSize = pRenderer.bounds.size;
				
				//align projector to longest dimension of prefab
				if (boxSize.z > boxSize.x) {
					rotateObjOnY(lightObj, 90f);
				}
				//match size of prefab
				lightProj.orthoGraphicSize = boxSize.x / 2f;
				lightProj.aspectRatio = boxSize.z / boxSize.x;
				
				startedPlacing = false;
			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				placingTurret = false;
			}

			//right mouse click to rotate placing position
			if (Input.GetMouseButtonDown (1)) {
				rotateObjOnY(lightObj, 90f);
			}

			//cast ray from camera to ground, get intersection point and move light there
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
				Vector3 pos = new Vector3(hitInfo.point.x, 1f, hitInfo.point.z);
				lightObj.transform.position = pos;
				
				//TODO: check for collisions here (and make projector light red)
				
				//TODO: replace discardClick
				if (Input.GetMouseButtonDown (0) && !discardClick) {
					CreateTurret (selectedTurret, hitInfo.point, lightObj.transform.rotation.eulerAngles.y - 90f);
					lightProj.enabled = false;
					placingTurret = false;
				}
			}
		}
	}
	
//	//disable all components but the render
//	void DisableAllButRender (GameObject obj)
//	{
//		//disable all scripts
//		foreach (Behaviour childComponent in obj.GetComponentsInChildren<Behaviour>()) {
//			childComponent.enabled = false;
//		}
//		obj.collider.enabled = false;
//		//TODO: ensure every other component is disabled (e.g. what about rigidbody?)
//	}
//	
//	void EnableAll (GameObject obj)
//	{
//		foreach (Behaviour childComponent in obj.GetComponentsInChildren<Behaviour>()) {
//			childComponent.enabled = true;
//		}
//		obj.collider.enabled = true;
//		//TODO: ensure every other component is enabled
//	}
	
	GameObject CreateTurret (int turretKind, Vector3 position, float yRotation)
	{
		GameObject prefab = (GameObject)placeablePrefabs[turretKind];
		
		//consume coins
		Placeable placeable = prefab.GetComponentsInChildren<Placeable>(true)[0];
		myGameInfo.coins -= placeable.price;
		
		//correct placing position with height
		Transform pTransform = prefab.GetComponentsInChildren<Transform>(true)[0];
		Vector3 placingPosition = new Vector3(position.x, position.y + pTransform.position.y, position.z);
		
		//create and place turret
		Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
		return (GameObject)Instantiate(prefab, placingPosition, rotation);
	}
}
