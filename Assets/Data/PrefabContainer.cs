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

    // Textures

    // Sound Effects

    // Background Music

}
