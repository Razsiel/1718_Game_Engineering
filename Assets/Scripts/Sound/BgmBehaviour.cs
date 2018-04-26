using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BgmBehaviour : TGEMonoBehaviour {

    private AudioSource bgmPlayer;
    private Dictionary<BGM, AudioClip> MusicClips;

    public override void Awake()
    {
        EventManager.OnAudioInitialize += Initialize;
    }

    void Initialize()
    {
        bgmPlayer = GetComponent<AudioSource>();

        MusicClips = new Dictionary<BGM, AudioClip>();
        EventManager.OnPlayMusicClip += PlayMusicClip;
        bgmPlayer.volume = 0.5f;

        // Add all music clips to Dictionary
    }

    public void PlayMusicClip(BGM clipName)
    {
        bgmPlayer.clip = MusicClips[clipName];
        bgmPlayer.Play();
    }
}

public enum BGM
{
    
}