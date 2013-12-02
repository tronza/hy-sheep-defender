using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour
{
	public float moveSpeed = 10f; //in meters per second
	public float rotationSpeed = 180.0F; //in degrees per sec
	public float jumpSpeed = 20.0F;
	public float jumpUpTime = 0.5f; //time a jump goes up in secs
	public int maxJumps = 2; //double jump allowed!
	public bool relativeMove = true;
	public Object[] weaponPrefabs;
	
	CharacterController cont;
	Vector3 moveDir;
	float advance;
	float rotation;
	float jumpTimeLeft;
	int jumpCount; //how many jumps have been done since touching the ground
	float gravity;
	Plane plane = new Plane(Vector3.up, 0F);
	int currentWeapon;
	
	void Start () {
		cont = GetComponent<CharacterController>();
		jumpTimeLeft = 0F;
		jumpCount = 0;
		gravity = Physics.gravity.magnitude;
		currentWeapon = 0;
	}
	
	void Update () {
		if(relativeMove){
			moveDir = Vector3.zero;
			
			advance = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
	        rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
			
			//if goind reverse, reverse rotation
			if(advance < 0F) {
				rotation = -rotation;
			}
			transform.Rotate(0F, rotation, 0F);
			
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
				
				float angle = Quaternion.Angle(transform.rotation, lookRotation);
				float timeToComplete = angle / rotationSpeed;
				
				//rotate towards a direction, but not immediately (rotate a little every frame)
				//The 3rd parameter is a number between 0 and 1, where 0 is the start rotation and 1 is the end rotation
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime / timeToComplete);
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
		//TODO: calculate real gravity acceleration
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
		
		//weapon switching
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0F){
			if (scroll > 0F) {
				++currentWeapon;
			} else {
				--currentWeapon;
			}
			currentWeapon %= weaponPrefabs.Length;
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
