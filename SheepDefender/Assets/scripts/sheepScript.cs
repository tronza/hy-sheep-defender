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
	
	/* Start() : is used for initialization
	 * Nothing
	 * */
	void Start () {
		
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
				if(currentWeapon==1) //red lazer
				{
					Instantiate (lazer_red_prefab, new Vector3(gameObject.transform.localPosition.x,gameObject.transform.localPosition.y+0.8F,gameObject.transform.localPosition.z), transform.rotation);
				}
				else if(currentWeapon==2) //green lazer
				{
					Instantiate (lazer_green_prefab, new Vector3(gameObject.transform.localPosition.x,gameObject.transform.localPosition.y+0.8F,gameObject.transform.localPosition.z), transform.rotation);
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
}