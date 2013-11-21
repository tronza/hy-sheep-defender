using UnityEngine;
using System.Collections;

public class lazerScript : MonoBehaviour {
	public float damage = 5f; //damage delt by the lazer
	public int speed = 16; //lazer speed
	
	// TODO: Why is this public?
	public GameObject playerSheep;

	/* Update() : is called once per frame
	 * move the lazer
	 * */
	void Update () {
		gameObject.transform.Translate(0,0,speed*Time.deltaTime);
	}
	
	/* OnCollisionEnter(Collision collision):
	 * deal damage to the attacker if it collide with one
	 * */
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Attacker") 
		{
			collision.gameObject.SendMessage("ReceiveDamage", damage);
			playerSheep = GameObject.Find ("Sheep");
			
			if (playerSheep != null) {
				collision.gameObject.SendMessage ("ChangeTarget", playerSheep.transform);
			}
		}
		
		//does not destroy itself if it collide with the player
		// TODO: Should these be Sheep/sheep or Target/target ???
		if (!(collision.gameObject.name.Contains("heep") && !(collision.gameObject.name.Contains("arget")))) 
		{
			Destroy(gameObject);
		}
	}
}
