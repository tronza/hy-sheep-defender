using System;

/**
 * Copyright 2013-2014 Jannis Seemann, Mika Hämäläinen
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
/**
 * When using the Player Pref class, use this enums keys to identify the
 */
public sealed class PlayerPrefKeys
{
	private PlayerPrefKeys() {}
	
	/**
	 * PayerPrefs.SetInt.
	 * 
	 * 0 => game is still running
	 * 1 => gameover
	 */
	public const string LEVEL_GAMEOVER = "level_gameover";
	
	/**
	 * The current level filename.
	 * 
	 * PayerPrefs.SetString.
	 */
	public const string LEVEL_CURRENT = "level_current";
	
	/* The current score of the Player */
	public const string SCORE = "score";
	
	//For PayerPrefs, is mute set
	public const string MUTE = "mute";
	
	public const string SKIP_TITLE = "skip_title";
	
}

