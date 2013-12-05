using UnityEngine;
using System.Collections;

/*
 * This class turns sound on or off accordingly.
 * At the beginning, the saved mute state is loaded and set.
 * Can be placed anywhere in a scene. 
 * Set the main camera to cameraWithAudioListener (or any object that has the audiolistener).
 *
 * */
public class Mute : MonoBehaviour
{
	public GameObject cameraWithAudioListener;
	private bool muteStatus = false;
	// Use this for initialization
	void Awake ()
	{
		int mute = PlayerPrefs.GetInt (PlayerPrefKeys.MUTE,0);//1-> mute, 0 -> sound on
		if (mute == 0) {
			SetMute (false);
		} else {
			SetMute (true);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void SetMute (bool soundOff)
	{
		
		AudioListener audio = cameraWithAudioListener.GetComponent<AudioListener> ();
		AudioSource music = cameraWithAudioListener.GetComponent<AudioSource> ();
		if (soundOff) {
			PlayerPrefs.SetInt (PlayerPrefKeys.MUTE, 1);
			audio.enabled = false;
			music.enabled = false;
			muteStatus = false;
		} else {
			PlayerPrefs.SetInt (PlayerPrefKeys.MUTE, 0);
			audio.enabled = true;
			music.enabled = true;
			muteStatus = true;
		}
	}

	public bool GetMute ()
	{
		return this.muteStatus;
	}
	
}
