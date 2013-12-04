using UnityEngine;
using System.Collections;

public class coinScript : MonoBehaviour {
	public GameObject dieEffect; //effect used when the coin disappear
	public int coinValue = 3; //The value of the coin
	
	double destruction; //time when the coin has to disappear
	double timeRemaning = 5F; //time the coin stays in game
	
	GameInfo gameInfo;//The info object to set the coins
	
	/* Start() : is used for initialization
	 * set the destruction time
	 * */
	void Start () {
		destruction=Time.time + timeRemaning;
		GameObject gameInfoObject = GameObject.Find("GameInfoObject");
		gameInfo = gameInfoObject.GetComponent<GameInfo>();
	}
	
	/* Update() : is called once per frame
	 * test the time until it has to destroy itself
	 * */
	void Update () {
		if(Time.time>=destruction)
		{
			Destroy(gameObject);
			Destroy (Instantiate (dieEffect, transform.position, Quaternion.identity), 0.5F);
		}
	}
	
	/* Collected() : is called by the sheep
	 * destroys itself
	 * */
	public void Collected()
	{
			gameInfo.coins = gameInfo.coins + coinValue;
			Destroy(gameObject);
	}
}
