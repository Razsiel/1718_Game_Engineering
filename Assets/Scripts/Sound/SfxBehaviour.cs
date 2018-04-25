using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SfxBehaviour : MonoBehaviour
{
    private AudioSource sfxPlayer;
    private PrefabContainer prefabContainer;

    private Dictionary<SFX, AudioClip> SoundEffects;

    void Awake()
    {
        EventManager.OnAudioInitialize += Initialize;
    }

	void Initialize ()
	{
        print("audio init");
	    sfxPlayer = GetComponent<AudioSource>();
	    prefabContainer = GameManager.GetInstance().PrefabContainer;

	    SoundEffects = new Dictionary<SFX, AudioClip>();
        EventManager.OnPlaySoundEffect += PlaySfx;
	    sfxPlayer.volume = 0.5f;

        // Add all sound effects to Dictionary
        SoundEffects.Add(SFX.ButtonHover, prefabContainer.sfx_button_hover);
	}
    
    public void PlaySfx(SFX soundName)
    {
        AudioClip soundClip = SoundEffects[soundName];
        sfxPlayer.PlayOneShot(soundClip);
    }
}

public enum SFX
{
    ButtonHover
}
