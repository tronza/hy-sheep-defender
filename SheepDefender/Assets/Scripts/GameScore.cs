using UnityEngine;
using System.Collections;

public class GameScore : MonoBehaviour
{
	public GameScore ()
	{
	}
	
	public void Start () 
	{
		PlayerPrefs.SetInt (PlayerPrefKeys.SCORE, 0);	
	}
	
	public void OnGUI () 
	{
		var scorePoints = PlayerPrefs.GetInt (PlayerPrefKeys.SCORE);

		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
			GUILayout.BeginVertical ();
				GUILayout.BeginHorizontal ();
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Score: " + scorePoints.ToString());
				GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
}
