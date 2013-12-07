using UnityEngine;
using System.Collections;

public class Detacheable : MonoBehaviour {

	public void ParentAboutToBeDestroyed() {
		Debug.Log("My parent was about to be destroyed, so I moved out.");
		transform.parent = null;
	}
}
