using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour
{
	public float moveSpeed = 10f; //in meters per second
	public float rotationSpeed = 180.0F; //in degrees per sec
	public float jumpSpeed = 20.0F;
	public float jumpUpTime = 0.5f; //time a jump goes up in secs
	public int maxJumps = 2; //double jump allowed!
	public bool absoluteKeyMove = false;
	public Transform gunHolder;
	public Object[] weaponPrefabs;
	public GameInfo gameInfo; //TODO: make singleton
	public float sensitivityX = 15F;
	public enum MovementMode {Stopped, NoMouse, LookToMouse, DeltaMouse}
	
	CharacterController controller;
	Vector3 moveDir;
	float advance;
	float rotation;
	float jumpTimeLeft;
	int jumpCount; //how many jumps have been done since touching the ground
	float gravity;
	Plane plane = new Plane(Vector3.up, 0F);
	int selectedWeapon;
	Weapon weapon;
	MovementMode movementMode;
	string targetSheep = "TargetSheep";
	
	void SwitchWeapon() {
		Object wPrefab = weaponPrefabs[selectedWeapon];
		if (weapon != null) {
			Destroy(weapon);
		}
		//this will ignore any base transform the weapon prefab has (add code if you want to conserve it)
		GameObject weaponObj = (GameObject)Instantiate(wPrefab, gunHolder.position, gunHolder.rotation);
		weapon = weaponObj.GetComponent<Weapon>();
		weaponObj.transform.parent = gunHolder;
	}
	
	void Start () {
		movementMode = MovementMode.DeltaMouse;
		controller = GetComponent<CharacterController>();
		jumpTimeLeft = 0F;
		jumpCount = 0;
		gravity = Physics.gravity.magnitude;
		selectedWeapon = 0;
		SwitchWeapon();
	}
	
	public void SetMovementMode(MovementMode mode) {
		movementMode = mode;
	}
	
	void Update () {
		if (movementMode == MovementMode.Stopped) {
			return;
		}
		
		if (movementMode == MovementMode.NoMouse) {
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
			moveDir.x = Input.GetAxis("Horizontal");
			moveDir.y = 0F;
			moveDir.z = Input.GetAxis("Vertical");
			if (moveDir.magnitude > 1F) {
				moveDir.Normalize();
			}
			moveDir *= moveSpeed * Time.deltaTime;
			
			if (movementMode == MovementMode.DeltaMouse) {
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
				moveDir = Quaternion.LookRotation(transform.forward) * moveDir;
			} else if (movementMode == MovementMode.LookToMouse) {
				
				Debug.Log("DOING THE WRONG THING");
				//cast ray from camera to plane (plane is at ground level, but infinite in space)
				float dist;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (plane.Raycast(ray, out dist)) {
					Vector3 point = ray.GetPoint(dist);
					point.y = transform.position.y;
					
					//find the vector pointing from our position to the target
					Vector3 direction = (point - transform.position).normalized;
					
					//create the rotation we need to be in to look at the target
					Quaternion lookRotation = Quaternion.LookRotation(direction);
					
					if (transform.rotation != lookRotation) {
						float angle = Quaternion.Angle(transform.rotation, lookRotation);
						float timeToComplete = angle / rotationSpeed;
						float donePercentage = Mathf.Min(1F, Time.deltaTime / timeToComplete);
						
						//rotate towards a direction, but not immediately (rotate a little every frame)
						//The 3rd parameter is a number between 0 and 1, where 0 is the start rotation and 1 is the end rotation
						transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, donePercentage);
					}
				}
				if (!absoluteKeyMove) {
					moveDir = Quaternion.LookRotation(transform.forward) * moveDir;
				}
			}
		}
		
		//if you are standing on the ground
		if(controller.isGrounded) {
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
		controller.Move(moveDir);
		
		//weapon firing
		if (Input.GetButton("Fire1")) {
			weapon.PullTrigger();
		} else if (Input.GetButtonUp("Fire1")) {
			weapon.ReleaseTrigger();
		}
		
		//weapon switching
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0F){
			if (scroll > 0F) {
				++selectedWeapon;
			} else {
				--selectedWeapon;
			}
			selectedWeapon = Mathf.Abs(selectedWeapon % weaponPrefabs.Length);
			SwitchWeapon();
		}
	}
	
	//TODO: horrible, replace from coin point of view
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.gameObject.name.Contains ("coin")) {
			hit.gameObject.SendMessage ("Collected");
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		Collectible collectible = other.GetComponent<Collectible>();
		if (collectible != null) {
			gameInfo.coins += collectible.storeValue;
			Destroy(other.gameObject);
		}
	}
	public void Die(){
		gameObject.GetComponent<Damageable>().enabled = false;
		gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
		GameObject.Find(targetSheep).SetActive(false);
		this.enabled = false;
	}
}
