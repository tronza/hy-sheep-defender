using UnityEngine;
using System.Collections;

public class GameScore : MonoBehaviour
{
	public GameScore ()
	{
	}
	
	public void OnGUI () 
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
			GUILayout.BeginVertical ();
				GUILayout.BeginHorizontal ();
					GUILayout.FlexibleSpace ();
					GUILayout.Label (PlayerPrefs.GetString (PlayerPrefKeys.SCORE));
				GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
}
