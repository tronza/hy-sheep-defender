using UnityEngine;
using System.Collections;

public class sheepScript : MonoBehaviour {
	
	public Transform lazer_red_prefab; //first weapon : red lazer
	public Transform lazer_green_prefab; //second weapon : green lazer
	
	float speed = 10F; //sheep speed
	float fireRate =0.2F; //lazers rate
	double nextShot =0.0; //time when the next shot has to happen
	int angle = 0; //angle for rotation
	int amountAngle = 2; //angle changing during rotation
	int currentWeapon=1; //represents the weapon to use 1->red lazer, 2->green lazer
	GameObject collidedWith ; //represents the object it is colliding with
	
	private Vector3 moveDirection = Vector3.zero;
	
	public GameObject theGun;
	
	public float damage = 5f;
	
	/* Attribute for Energy bar */
	public int maxCooldown = 100; // Maximum Energy of sheep
	public int curCooldown = 100; // Current Energy of sheep
	public float lengthCooldown; // Length Energy bar of sheep
	
	// For the style of Energy bar
	GUIStyle style; 
    Texture2D texture;
    Color redColor;
    Color greenColor;
	
	// For incrementation of Energy bar after n seconds
	float incrementTime = 1f;
	float incrementBy = 1;
	double time = 0;
	
	
	void Start() {
		// Initialize Energy bar and its style
		lengthCooldown=Screen.width / 3;
		texture = new Texture2D(1, 1);
        texture.SetPixel(1, 1, greenColor);
		
		style = new GUIStyle();
		redColor = Color.red;
    	greenColor = Color.green;
		
	}
	
	/* Update() : is called once per frame
	 * modify rotation and translation regarding the input
	 * shoot lazer if we press "Jump"
	 * change weapon if we scrool up
	 * */
	void Update () {	
		
		/*CharacterController controller = GetComponent<CharacterController>();      
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;            
        controller.Move(moveDirection * Time.deltaTime);*/
		
		/* Rotation */
		//TODO Modify rotation 
		if(Input.GetAxis("Horizontal")!=0)
        {
            if(angle>360 || angle<-360)
			{	
				angle=0;
			}
			if(Input.GetAxis("Horizontal")>0)
			{
				angle+=amountAngle;
			}
			if(Input.GetAxis("Horizontal")<0)
			{
				angle-=amountAngle;
			}
			
			gameObject.transform.rotation = Quaternion.Euler(0,angle,0);
        }
		
		/* Forward and backward */ 
		if(Input.GetAxis("Vertical")!=0)
        {
			if (collidedWith!=null && !(collidedWith.tag == "Attacker") && !(collidedWith.name.Contains("lazer")) )
			{
				return;
			}
			
			gameObject.transform.Translate(0,0,Input.GetAxis("Vertical")*speed*Time.deltaTime);
			
        }
		
		/*shooting lazer*/
		if(Input.GetAxis("Jump")!=0)
        {
			if(Time.time >= nextShot)
			{
				if(currentWeapon==1 && curCooldown>0) //red lazer
				{
					Instantiate (lazer_red_prefab, theGun.transform.position, theGun.transform.rotation);
					curCooldown--;
				}
				else if(currentWeapon==2 && curCooldown>0) //green lazer
				{
					Instantiate (lazer_green_prefab, theGun.transform.position, theGun.transform.rotation);
					curCooldown=curCooldown-2;
				}
				nextShot = Time.time + fireRate; //set the next shot time
			}
        }
		
		/*changing weapon*/
		if(Input.GetAxis("Mouse ScrollWheel")>0)
		{
			currentWeapon++;
			if(currentWeapon>2)
			{
				currentWeapon=1;
			}
		}
		
		/* Increment the current Energy after n seconds */
		time+=Time.deltaTime;
		if (time >= incrementTime)
		{
			curCooldown++;
			time=0;
		}
		
		/* Adjust to Energy bar of the sheep */ 
		AdjustCurrentHealth(0);
	}
	
	/* OnCollisionEnter(Collision collision):
	 * set the collidedWith variable
	 * if we collide with a coin -> send a message to the coin so it destroys itself
	 * then add coins to the total of coins
	 * */
	void OnCollisionEnter(Collision collision) {
		collidedWith=collision.gameObject;
		if(collision.gameObject.name.Contains("coin"))
		{
			collision.gameObject.SendMessage("Collected");
			this.AddCoin();
		}
	}
	
	/* OnCollisionExit(Collision collision) :
	 * set the collidedWith variable to null
	 * */
	void OnCollisionExit(Collision collision){
		collidedWith=null;
	}
	
	/* AddCoin():
	 * generate a random number (integer)
	 * add this number to the total amount of coins
	 * */
	void AddCoin(){
		
	}
	
	/*OnGUI(): 
	 * Print the Energy of the sheep in the screen
	 * 
	 * */
	void OnGUI()
	{
		texture.Apply();
 
        style.normal.background = texture;
		
		GUI.Box(new Rect(0,200,lengthCooldown,20), new GUIContent(""),style);
	}
	
	/*
	 * Adjust the display of the Energy bar
	 * */
	
	public void AdjustCurrentHealth(int adj) { 
 
	if (curCooldown < 0) 
		curCooldown = 0; 
	
	if (curCooldown > maxCooldown) 
		curCooldown = maxCooldown;
	
	// No division by zero
	if (maxCooldown < 1)
		maxCooldown = 1;
	
	if (curCooldown > 50)
	{
		texture.SetPixel(1, 1, greenColor);
	}
	
	if (curCooldown < 50)
	{
		texture.SetPixel(1, 1, redColor);
	}
	
	lengthCooldown=(Screen.width / 3) * (curCooldown / (float)maxCooldown);
	}
}
