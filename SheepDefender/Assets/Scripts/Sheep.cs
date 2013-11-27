using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour
{
	
	//TODO: should be an array of weapons
	
	int currentWeapon = 1; //represents the weapon to use 1->red lazer, 2->green lazer
	
	public float moveSpeed = 10f;
	public float rotationSpeed = 100.0F; //in degrees per sec
	float gravity = 9.81f;
	public float jumpHeight = 4f;
	int jumpCount = 0;
	public int jumpCap = 2; //double jump allowed!
	Vector3 moveDir;
//	Transform cam;
	CharacterController cont;
	
	float advancement;
	float rotation;
	
	void Start () {
		cont = GetComponent<CharacterController>();
	}
	
	void Update () {
		moveDir = Vector3.zero;
		
		if(cont.isGrounded) {
			moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			jumpCount = 0;
			
			if(Input.GetButtonDown("Jump") && jumpCount <= jumpCap) {
				jumpCount+= 1;
				moveDir.y += jumpHeight;
			}
			
			moveDir *= moveSpeed;
		}
		else if(transform.position.y > -1.0f) {
			moveDir.y -= gravity;
		}
		
//		Debug.Log(moveDir);
		cont.Move(moveDir * Time.deltaTime);
		
		
		//cast ray from camera to ground, get intersection point with ground layer and move light there
//		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//		RaycastHit hitInfo;
//		
//		//allow for plane
//		if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, groundLayerMask)) {lightObj.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + placeablePosY, hitInfo.point.z);
//			//
//		}
		
		
		/*changing weapon*/
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			currentWeapon++;
			if (currentWeapon > 2) {
				currentWeapon = 1;
			}
		}
	}
	
	//TODO: horrible, replace from coin point of view
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.gameObject.name.Contains ("coin")) {
			hit.gameObject.SendMessage ("Collected");
		}
	}
}
