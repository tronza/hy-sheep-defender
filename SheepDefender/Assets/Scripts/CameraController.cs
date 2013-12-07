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
		if (Input.GetKeyDown ("c")) {
			ChangeCamera ();
		}
	}
	
	public void ActivateMainCamera ()
	{
		if (playerSheep != null) {
			playerSheep.GetComponent<Sheep> ().SetMovementMode (Sheep.MovementMode.Stopped);
		}
		if (thirdPersonCamera != null) {
			thirdPersonCamera.GetComponent<Camera> ().enabled = false;
			mainCamera.GetComponent<Camera> ().enabled = true;
		}
	}
	
	public void ActivateThirdPersonCamera ()
	{
		if (thirdPersonCamera != null) {
			mainCamera.GetComponent<Camera> ().enabled = false;
			thirdPersonCamera.GetComponent<Camera> ().enabled = true;
		}
		if (playerSheep != null) {
			playerSheep.GetComponent<Sheep> ().SetMovementMode (Sheep.MovementMode.DeltaMouse);
		}
	}
	
	public void ChangeCamera ()
	{
		if (mainCamera.GetComponent<Camera> ().enabled) {
			ActivateThirdPersonCamera ();
		} else {
			ActivateMainCamera ();
		}
	}
}
