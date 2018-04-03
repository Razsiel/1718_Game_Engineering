﻿using UnityEngine;
using Assets.ScriptableObjects.Levels;
using System.Collections.Generic;
using Assets.Scripts.Photon;
using Assets.ScriptableObjects;
using Assets.ScriptableObjects.Player;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    
    public GameObject PlayerPrefab;
    public List<PlayerData> Players;

    public CommandLibrary CommandLibrary;
    public LevelData LevelData;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
        
        // Setup level
        LevelData.Init(Players);
    }

    public void StartMultiplayerGame()
    {
        //Lets do some GameStarting logic here
        PhotonManager.Instance.StartMultiplayerGame(LevelData);
    }

  

    public static GameManager GetInstance() {
        return _instance;
    }
}