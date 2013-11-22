using UnityEngine;
using System.Collections;

public class MuteButton : MonoBehaviour
{
	
	private Mute muter;
	private bool muteStatus = false;
	public Texture muteOffTexture;
	public Texture muteOnTexture;
	// Use this for initialization
	void Start ()
	{
		muter = gameObject.GetComponent<Mute> ();
		ChangeTexture(muter.GetMute());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnMouseDown ()
	{
		muter.SetMute(!muteStatus);
		ChangeTexture(!muteStatus);
	}
	
	void ChangeTexture (bool muted)
	{
		muteStatus = muted;
		if (muted) {
			gameObject.guiTexture.texture = muteOnTexture;
		}else{
			gameObject.guiTexture.texture = muteOffTexture;
		}
	}
}
