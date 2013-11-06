using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
	public float damage = 10f;
	public float cooldownTime = 2f; // Seconds
	GameObject target = null;
	float cooldownRemaining = 0f;
	
	void Start ()
	{
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
	    lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
	    lineRenderer.SetColors(Color.cyan, Color.blue);
	    lineRenderer.SetWidth(0.1f, 0.1f);
	    lineRenderer.SetVertexCount(2);
	}

	void Update ()
	{
		if (this.HasTargetAcquired ()) {
			transform.LookAt (this.target.transform.position);
		}
		
		if (cooldownRemaining <= (cooldownTime/2)) {
			LineRenderer lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.enabled = false;
		}
		
		// TODO: it takes one extra frame (!) to find a new target
		if (cooldownRemaining <= 0) {
			if (this.HasTargetAcquired ()) {
				DealDamage ();
				cooldownRemaining = cooldownTime;
			} else {
				this.AcquireNextTarget ();
			}
		}
		
		cooldownRemaining -= Time.deltaTime;
	}
	
	void DealDamage ()
	{
		if (this.target != null) {
			RaycastHit hit;
        	
			if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit)) {
				// It really does not matter if the target is the target, only the classification matters here.
				try {
					if (hit.collider.gameObject.tag == this.target.tag) {
						Debug.DrawLine (transform.position, hit.transform.position, Color.yellow, 0.5f, false);
						
						LineRenderer lineRenderer = GetComponent<LineRenderer>();
						lineRenderer.enabled = true;
						lineRenderer.SetPosition (0, transform.position);
						lineRenderer.SetPosition (1, hit.transform.position);
						
						hit.collider.gameObject.SendMessage ("ReceiveDamage", damage);
					}
				} catch (UnityException e) {
					Debug.Log (e.Message);
				}
			}
			
			if (this.target != null) {
				this.AcquireNextTarget ();	
			}
		} else {
			this.target = null;
			
			this.AcquireNextTarget ();
		}
	}
	
	bool HasTargetAcquired ()
	{
		if (this.target != null) {
			return true;
		}
		
		return false;
	}
	
	void AcquireNextTarget ()
	{
		this.target = this.FindTarget ();
	}
	
	bool canBeAttacked (GameObject go)
	{
		RaycastHit hit;
		
		if (Physics.Raycast (transform.position, (go.transform.position - transform.position), out hit)) {
			if (hit.collider.gameObject == go) {
				return true;
			}
		}
		
		return false;
	}
	
	GameObject FindTarget ()
	{
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Attacker")) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			
			if ((curDistance < distance) && (canBeAttacked (go))) {
				closest = go;		
				distance = curDistance;
			}
		}
		
		return closest;
	}
}
