using UnityEngine;
using System.Collections;

public class GameScoreable : MonoBehaviour
{
	/**
	 * The number of score points for this object. 
	 */
	public int scorePointsBeginning = 5;
	
	/**
	 * Decrease the score Points every ... seconds by one till only one score point 
	 * is left.
	 */
	public int decreaseScorePointEvery = 10;
	
	private int existsSince = 0;
	
	public GameScoreable ()
	{
	}
	
	public void Start () 
	{
		this.existsSince = Mathf.FloorToInt (Time.realtimeSinceStartup);	
	}
	
	public void HealthZeroed () 
	{
		var scorePoints = this.scorePointsBeginning + 1;
		var decreaseBy = Mathf.FloorToInt ((Mathf.FloorToInt (Time.realtimeSinceStartup) - existsSince) / this.decreaseScorePointEvery);
		var addScorePoints = scorePoints - Mathf.Max (1, decreaseBy);

		PlayerPrefs.SetInt (PlayerPrefKeys.SCORE, PlayerPrefs.GetInt(PlayerPrefKeys.SCORE) + addScorePoints);
	}
}
