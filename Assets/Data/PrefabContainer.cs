using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contianer for Prefabs, Textures, etc.
/// Provides hard-links to access project-files.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "PrefabContainer", menuName = "Data/PrefabContainer")]
public class PrefabContainer : ScriptableObject {

    // Prefabs
    [SerializeField] public GameObject PlayerPrefab;

    // Images
    [SerializeField] public Sprite ImageNotFound;

    [SerializeField] public Sprite MoveCommand;
    [SerializeField] public Sprite TurnCommandLeft;
    [SerializeField] public Sprite TurnCommandRight;
    [SerializeField] public Sprite InteractCommand;
    [SerializeField] public Sprite WaitCommand;

    [SerializeField] public Sprite PlayButton;
    [SerializeField] public Sprite ReadyButton;
    [SerializeField] public Sprite StopButton;


    // Textures

    // Sound Effects
    [SerializeField] public AudioClip sfx_button_hover;

    // Background Music


    public void Initialize()
    {
        sfx_button_hover = Resources.Load<AudioClip>("Sound/SFX/sfx_button_hover");
        PlayButton = Resources.Load<Sprite>("Images/Img_Play_Temp");
        ReadyButton = Resources.Load<Sprite>("Images/Img_Ready_Temp");
        StopButton = Resources.Load<Sprite>("Images/Img_Stop_Temp");

    }
}
