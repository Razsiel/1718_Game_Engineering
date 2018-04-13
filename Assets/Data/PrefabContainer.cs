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
    [SerializeField] public Texture2D ImageNotFound;

    [SerializeField] public Texture2D MoveCommand;
    [SerializeField] public Texture2D TurnCommandLeft;
    [SerializeField] public Texture2D TurnCommandRight;
    [SerializeField] public Texture2D InteractCommand;
    [SerializeField] public Texture2D WaitCommand;

    // Textures

    // Sound Effects
    [SerializeField] public AudioClip sfx_button_hover;

    // Background Music


    public void Initialize()
    {
        sfx_button_hover = Resources.Load<AudioClip>("Sound/SFX/sfx_button_hover");
    }
}
