using UnityEngine;
using System.Collections;

/**
 * Copyright 2013-2014 Mika Hämäläinen
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
