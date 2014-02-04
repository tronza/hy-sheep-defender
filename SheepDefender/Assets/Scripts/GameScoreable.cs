using UnityEngine;
using System.Collections;

/**
 * Copyright 2014 Jannis Seemann, Agostino Sturaro
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
