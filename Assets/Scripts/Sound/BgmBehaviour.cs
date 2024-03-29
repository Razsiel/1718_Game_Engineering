﻿using System.Collections;
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
        
        bgmPlayer = GetComponent<AudioSource>();

        MusicClips = new Dictionary<BGM, AudioClip>()
        {
            {BGM.MainTheme, Resources.Load<AudioClip>("Sound/BGM/Main_Theme")}
        };
    }

    public override void Start()
    {
        PlayMusicClip(BGM.MainTheme);
    }

    public static void PlayMusicClip(BGM clipName)
    {
        Instance.bgmPlayer.clip = Instance.MusicClips[clipName];
        Instance.bgmPlayer.Play();
    }

    public void SetVolume(float newVolumeValue)
    {
        bgmPlayer.volume = newVolumeValue;
        PlayerPrefs.SetFloat("BGM Volume", bgmPlayer.volume);
    }
}

public enum BGM
{
    MainTheme
}