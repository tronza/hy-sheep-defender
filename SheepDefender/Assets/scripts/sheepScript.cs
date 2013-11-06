using UnityEngine;
using System.Collections;

public class sheepScript : MonoBehaviour {
	int speed = 10;
	float fireRate =0.2F;
	double nextShot =0.0;
	int angle = 0;
	int ajoutAngle = 2;
	GameObject collidedWith ;
	
	public Transform lazer_prefab;
	
	public float damage = 5f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		/* Rotation */
		if(Input.GetAxis("Horizontal")!=0)
        {
            if(angle>360 || angle<-360)
			{	
				angle=0;
			}
			if(Input.GetAxis("Horizontal")>0)
			{
				angle+=ajoutAngle;
			}
			if(Input.GetAxis("Horizontal")<0)
			{
				angle-=ajoutAngle;
			}
			
			gameObject.transform.rotation = Quaternion.Euler(0,angle,0);
        }
		
		/* Forward and backward */ 
		if(Input.GetAxis("Vertical")!=0)
        {
			if (collidedWith!=null && !(collidedWith.tag == "Attacker"))
			{
				return;
			}
			
			gameObject.transform.Translate(0,0,Input.GetAxis("Vertical")*speed*Time.deltaTime);
			
        }
		
		if(Input.GetAxis("Jump")!=0)
        {
			if(Time.time >= nextShot)
			{
				Instantiate(lazer_prefab,new Vector3(gameObject.transform.localPosition.x,gameObject.transform.localPosition.y+2,gameObject.transform.localPosition.z),gameObject.transform.localRotation);
				nextShot = Time.time + fireRate;
			}
        }
	}
	
	void OnCollisionEnter(Collision collision) {
		collidedWith=collision.gameObject;
	}
	
	void OnCollisionExit(Collision collision){
		collidedWith=null;
	}
}