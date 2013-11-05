using UnityEngine;
using System.Collections;

public class TurretPlacer : MonoBehaviour
{
	
	//NOTE: you could use a Transform reference, but this is clearer
	public Object turretPrefab;
	
	//private vars
	Vector3 mousePosition;
	Plane plane = new Plane (Vector3.up, 0);
	float dist;
	Ray ray;
	
	//returned value is to say if everything went fine, or not
	public bool createTurret (int turretKind)
	{
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (plane.Raycast (ray, out dist)) {
			Vector3 point = ray.GetPoint (dist);
			Instantiate (turretPrefab, new Vector3 (point.x, point.y, 0), Quaternion.identity);
			return true;
		}
		else {
			return false;
		}
		
		//TODO: method to move turret while it is positioned
		//NOTE: alternative is to use a "ghost" turret and then instantiate a real turret
	}
}
