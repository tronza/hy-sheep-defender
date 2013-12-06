using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public GameObject mainCamera;
	public GameObject thirdPersonCamera;
	public GameObject playerSheep;
	
	// Use this for initialization
	void Start ()
	{
		ActivateThirdPersonCamera ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerPrefs.GetInt (PlayerPrefKeys.LEVEL_GAMEOVER) == 0) {
			if (Input.GetKeyDown ("c")) {
				ChangeCamera ();
			}
		}
	}
	
	public void ActivateMainCamera ()
	{
		if (PlayerPrefs.GetInt (PlayerPrefKeys.LEVEL_GAMEOVER) == 0) {
			playerSheep.GetComponent<Sheep> ().SetMovementMode (Sheep.MovementMode.Stopped);
	
			thirdPersonCamera.GetComponent<Camera> ().enabled = false;
			mainCamera.GetComponent<Camera> ().enabled = true;
		}
	}
	
	public void ActivateThirdPersonCamera ()
	{
		if (PlayerPrefs.GetInt (PlayerPrefKeys.LEVEL_GAMEOVER) == 0) {
			mainCamera.GetComponent<Camera> ().enabled = false;
			thirdPersonCamera.GetComponent<Camera> ().enabled = true;
	
			playerSheep.GetComponent<Sheep> ().SetMovementMode (Sheep.MovementMode.DeltaMouse);
		}
	}
	
	public void ChangeCamera ()
	{
		if (PlayerPrefs.GetInt (PlayerPrefKeys.LEVEL_GAMEOVER) == 0) {
			if (mainCamera.GetComponent<Camera> ().enabled) {
				ActivateThirdPersonCamera ();
			} else {
				ActivateMainCamera ();
			}
		}
	}
}
