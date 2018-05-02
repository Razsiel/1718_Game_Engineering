using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SfxBehaviour : MonoBehaviour
{
    private AudioSource sfxPlayer;

    private Dictionary<SFX, AudioClip> SoundEffects;

    public static SfxBehaviour Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        EventManager.OnAudioInitialize += Initialize;
    }

	void Initialize ()
	{
        print("audio init");
	    sfxPlayer = GetComponent<AudioSource>();

	    SoundEffects = new Dictionary<SFX, AudioClip>()
	    {
	        {SFX.ButtonHover, Resources.Load<AudioClip>("Sound/SFX/sfx_button_hover")}
	    };
	    sfxPlayer.volume = 0.5f;
	}
    
    public static void PlaySfx(SFX soundName)
    {
        AudioClip soundClip = Instance.SoundEffects[soundName];
        Instance.sfxPlayer.PlayOneShot(soundClip);
    }
}

public enum SFX
{
    ButtonHover
}
