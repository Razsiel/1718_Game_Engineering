using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SfxBehaviour : MonoBehaviour
{
    private AudioSource sfxPlayer;
    private PrefabContainer prefabContainer;

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
	    prefabContainer = null; //GameManager.GetInstance().PrefabContainer;

	    SoundEffects = new Dictionary<SFX, AudioClip>();
	    sfxPlayer.volume = 0.5f;

        // Add all sound effects to Dictionary
        SoundEffects.Add(SFX.ButtonHover, prefabContainer.sfx_button_hover);
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
