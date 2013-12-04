using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Camera mainCamera;
	public Camera thirdPersonCamera;
	public GameObject playerSheep;
	
	// Use this for initialization
	void Start ()
	{
		ActivateThirdPersonCamera ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("c")) {
			ChangeCamera ();
		}
	}
	
	public void ActivateMainCamera ()
	{
		playerSheep.GetComponent<Sheep>().SetMovementMode(Sheep.MovementMode.Stopped);

		thirdPersonCamera.gameObject.SetActive (false);
		mainCamera.gameObject.SetActive (true);
	}
	
	public void ActivateThirdPersonCamera ()
	{
		mainCamera.gameObject.SetActive (false);
		thirdPersonCamera.gameObject.SetActive (true);

		playerSheep.GetComponent<Sheep>().SetMovementMode(Sheep.MovementMode.DeltaMouse);
	}
	
	public void ChangeCamera ()
	{
		if (mainCamera.gameObject.activeSelf) {
			ActivateThirdPersonCamera ();
		} else {
			ActivateMainCamera ();
		}
	}
}
