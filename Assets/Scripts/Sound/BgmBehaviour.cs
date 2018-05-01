using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BgmBehaviour : TGEMonoBehaviour {

    private AudioSource bgmPlayer;
    private Dictionary<BGM, AudioClip> MusicClips;

    public static BgmBehaviour Instance { get; private set; }

    public override void Awake()
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

    void Initialize()
    {
        bgmPlayer = GetComponent<AudioSource>();

        MusicClips = new Dictionary<BGM, AudioClip>();
        bgmPlayer.volume = 0.5f;

        // Add all music clips to Dictionary
    }

    public static void PlayMusicClip(BGM clipName)
    {
        Instance.bgmPlayer.clip = Instance.MusicClips[clipName];
        Instance.bgmPlayer.Play();
    }
}

public enum BGM
{
    
}