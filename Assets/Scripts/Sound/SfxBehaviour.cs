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
        
	    sfxPlayer = GetComponent<AudioSource>();

	    SoundEffects = new Dictionary<SFX, AudioClip>()
	    {
	        {SFX.ButtonHover, Resources.Load<AudioClip>("Sound/SFX/sfx_button_hover")}
	    };
	}
    
    public static void PlaySfx(SFX soundName)
    {
        AudioClip soundClip = Instance.SoundEffects[soundName];
        Instance.sfxPlayer.PlayOneShot(soundClip);
    }

    public void SetVolume(float newVolumeValue)
    {
        sfxPlayer.volume = newVolumeValue;
        PlayerPrefs.SetFloat("SFX Volume", sfxPlayer.volume);
    }
}

public enum SFX
{
    ButtonHover
}
