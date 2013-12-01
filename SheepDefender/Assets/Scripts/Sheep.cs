using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour
{
	
	//TODO: should be an array of weapons
	
	int currentWeapon = 1; //represents the weapon to use 1->red lazer, 2->green lazer
	
	public float moveSpeed = 10f; //in meters per second
	public float rotationSpeed = 100.0F; //in degrees per sec
	public float jumpSpeed = 20.0F;
	public float jumpUpTime = 0.5f; //time a jump goes up in secs
	public int maxJumps = 2; //double jump allowed!
	public bool relativeMove = true;
	
	CharacterController cont;
	Vector3 moveDir;
	float advance;
	float rotation;
	float jumpTimeLeft;
	int jumpCount; //how many jumps have been done since touching the ground
	float gravity;
	Plane plane = new Plane(Vector3.up, 0);
	
	void Start () {
		cont = GetComponent<CharacterController>();
		jumpTimeLeft = 0F;
		jumpCount = 0;
		gravity = Physics.gravity.magnitude;
	}
	
	void Update () {
		if(relativeMove){
			moveDir = Vector3.zero;
			
			advance = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
	        rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
			
			//if goind reverse, reverse rotation
			if(advance < 0) {
				rotation = -rotation;
			}
			transform.Rotate(0, rotation, 0);
			
			//convert local forward to world forward and advance
	        moveDir = transform.TransformDirection(Vector3.forward);
	        moveDir *= advance;
		} else {
			moveDir.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
			moveDir.y = 0F;
			moveDir.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
			
			//cast ray from camera to plane (plane is at ground level, but infinite in space)
			float dist;
			Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
			if (plane.Raycast(ray, out dist)) {
				Vector3 point = ray.GetPoint(dist);
				
				//find the vector pointing from our position to the target
				Vector3 direction = (point - transform.position).normalized;
				
				//create the rotation we need to be in to look at the target
				Quaternion lookRotation = Quaternion.LookRotation(direction);
				
				//rotate towards a direction, but not immediately (rotate a little every frame)
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			}
		}
		//if you are standing on the ground
		if(cont.isGrounded) {
	        jumpCount = 0;
			jumpTimeLeft = jumpUpTime;
		} else {
			moveDir.y -= gravity * Time.deltaTime;
		}
		
		//if the jump button was just pressed
		if(Input.GetButtonDown("Jump")) {
			if(jumpCount < maxJumps) {
				jumpCount += 1;
				moveDir.y += jumpSpeed * Time.deltaTime;
			}
		} else if(Input.GetButton("Jump")) {
			if(jumpTimeLeft > 0F) {
				jumpTimeLeft -= Time.deltaTime;
				moveDir.y += jumpSpeed * Time.deltaTime;
			}
		}
		
		//takes absolute deltas, gravity must be applied by hand
		cont.Move(moveDir);
		
//			if(cont.isGrounded) {
//				moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
//				jumpCount = 0;
//				
//				if(Input.GetButtonDown("Jump") && jumpCount <= maxJumps) {
//					jumpCount+= 1;
//					moveDir.y += jumpHeight;
//				}
//				
//				moveDir *= moveSpeed;
//			}
//			else if(transform.position.y > -1.0f) {
//				moveDir.y -= gravity;
//			}
//			
//			cont.Move(moveDir * Time.deltaTime);
//			
//			//cast ray from camera to ground, get intersection point with ground layer and move light there
//			float dist;
//			Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
//			if (plane.Raycast(ray, out dist)) {
//				Vector3 point = ray.GetPoint(dist);
//				
//				//find the vector pointing from our position to the target
//				Vector3 _direction = (point - transform.position).normalized;
//				 
//				//create the rotation we need to be in to look at the target
//				Quaternion _lookRotation = Quaternion.LookRotation(_direction);
//				 
//				//rotate us over time according to speed until we are in the required rotation
//				transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
//			}
//			}
		
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
