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
	
	Plane plane = new Plane(Vector3.up,0);
	
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
		
		
		float dist;
		Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out dist)) {
			Vector3 point = ray.GetPoint(dist);
			
			//find the vector pointing from our position to the target
			Vector3 _direction = (point - transform.position).normalized;
			 
			//create the rotation we need to be in to look at the target
			Quaternion _lookRotation = Quaternion.LookRotation(_direction);
			 
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
		}
		
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
