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
	
	/* Attribute for health bar */
	public int maxHealth = 100; // Maximum Health of sheep
	public int curHealth = 100; // Current Health of sheep
	public float lengthBarHealth; // Length Health bar of sheep
	
	// For the style of health bar
	GUIStyle style; 
    Texture2D texture;
    Color redColor;
    Color greenColor;
	
	// For incrementation of health bar after n seconds
	float incrementTime = 2f;
	float incrementBy = 1;
	double time = 0;
	
	
	void Start() {
		// Initialize health bar and its style
		lengthBarHealth=Screen.width / 3;
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
				if(currentWeapon==1 && curHealth>0) //red lazer
				{
					Instantiate (lazer_red_prefab, theGun.transform.position, theGun.transform.rotation);
					curHealth--;
				}
				else if(currentWeapon==2 && curHealth>0) //green lazer
				{
					Instantiate (lazer_green_prefab, theGun.transform.position, theGun.transform.rotation);
					curHealth=curHealth-2;
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
		
		/* Increment the current health after n seconds */
		time+=Time.deltaTime;
		if (time >= incrementTime)
		{
			curHealth++;
			time=0;
		}
		
		/* Adjust to health bar of the sheep */ 
		AdjustCurrentHealth(0);
		
		// TODO Add if curHealth is 0 Destroyed the sheep
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
	 * Print the health of the sheep in the screen
	 * 
	 * */
	void OnGUI()
	{
		texture.Apply();
 
        style.normal.background = texture;
		
		GUI.Box(new Rect(0,200,lengthBarHealth,20), new GUIContent(""),style);
	}
	
	/*
	 * Adjust the display of the bar of life
	 * */
	
	public void AdjustCurrentHealth(int adj) { 
 
	if (curHealth < 0) 
		curHealth = 0; 
	
	if (curHealth > maxHealth) 
		curHealth = maxHealth;
	
	// No division by zero
	if (maxHealth < 1)
		maxHealth = 1;
	
	if (curHealth > 50)
	{
		texture.SetPixel(1, 1, greenColor);
	}
	
	if (curHealth < 50)
	{
		texture.SetPixel(1, 1, redColor);
	}
	
	lengthBarHealth=(Screen.width / 3) * (curHealth / (float)maxHealth);
	}
}
