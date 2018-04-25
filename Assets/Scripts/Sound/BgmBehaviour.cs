using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BgmBehaviour : MonoBehaviour {

    private AudioSource bgmPlayer;
    private PrefabContainer prefabContainer;

    private Dictionary<BGM, AudioClip> MusicClips;

    void Awake()
    {
        EventManager.OnAudioInitialize += Initialize;
    }

    void Initialize()
    {
        bgmPlayer = GetComponent<AudioSource>();
        prefabContainer = GameManager.GetInstance().PrefabContainer;

        MusicClips = new Dictionary<BGM, AudioClip>();
        EventManager._PlayMusicClip += PlayMusicClip;
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