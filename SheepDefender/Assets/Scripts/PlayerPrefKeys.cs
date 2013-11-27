using System;

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
	
	//For PayerPrefs, is mute set
	public const string MUTE = "mute";
	
	public const string SKIP_TITLE = "skip_title";
	
}

