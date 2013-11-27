using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	/**
	 * dieEffect may be null. Then the Damageable object will just... disappear.
	 */
	public GameObject dieEffect;
	public float health = 100f;
	public int numberOfCoins = 5;
	public Transform coin_prefab;
	
	float healthBarRatio;
	GameObject healthBar;
	//Vector3 healthBarStartPoint = new Vector3(-2.0f, 2.0f, 0.0f);
	//Vector3 healthBarEndPoint   = new Vector3(2.0f, 1.5f, 0.0f);
	
	MeshRenderer lineRenderer;
	
	// Use this for initialization
	void Start ()
	{
		healthBarRatio = health;
	    
		// Creates GameObject and a clone of it...
		healthBar = (GameObject) Instantiate (new GameObject(), transform.position, Quaternion.identity);
		healthBar.AddComponent<MeshRenderer>();
		healthBar.transform.LookAt(Camera.main.transform.position);
		
		lineRenderer = healthBar.GetComponent<MeshRenderer>();
		
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
	    /*lineRenderer.SetColors(Color.green, Color.green);
	    lineRenderer.SetWidth(0.25f, 0.25f);
	    lineRenderer.SetVertexCount(2);
		lineRenderer.enabled = false;*/
	}

	// Update is called once per frame
	void Update ()
	{
		if (lineRenderer.enabled) {	
			healthBar.transform.LookAt(Camera.main.transform.position, -Vector3.up);
			
			/**lineRenderer.SetPosition (0, transform.position + healthBarStartPoint);
			lineRenderer.SetPosition (1, transform.position + healthBarEndPoint);*/
		}
	}

	public void ReceiveDamage (float damage)
	{	
		// TODO: do something fancy with the amount of received damage ;-)
		// if (this.hasArmour ()) {
		// 	 this.health -= damage / 2;
		// } etc.
		
		float ratio = (healthBarRatio - damage) / healthBarRatio;
		
		healthBarEndPoint -= new Vector3(0.5f, 0.0f, 0.0f);
		
		this.health -= damage;
		
		if (!this.HasHealth ()) {
			// TODO: what happens to the referencing turrets etc after destroying the object?
			Destroy (healthBar);
			Destroy (gameObject);
			for (int i =0; i<numberOfCoins; i++) {
				if (gameObject.name.Contains ("Wolf")) {
					Instantiate (coin_prefab, new Vector3 (
					transform.localPosition.x+(0.5f*i),
					transform.localPosition.y + 0.5F,
					transform.localPosition.z+(0.25f*i)
				), Quaternion.identity);
				}
			}
			
			if (this.dieEffect) {
				Destroy (Instantiate (this.dieEffect, transform.position, Quaternion.identity), 1);	
			}
		} else {
			lineRenderer.enabled = true;
		}
	}

	public bool HasHealth ()
	{
		return this.health >= 0;
	}
}
