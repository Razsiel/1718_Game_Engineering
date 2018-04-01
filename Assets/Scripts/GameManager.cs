using UnityEngine;
using Assets.ScriptableObjects.Levels;
using System.Collections.Generic;
using Assets.Scripts.Photon;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    //Temp scene players (testing purposes)
    public Player PlayerA;
    public Player PlayerB;

    public GameObject PlayerPrefab;
    public List<Player> Players = new List<Player>();
    public CommandLibrary CommandLibrary;
    public LevelData LevelData;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        // Setup level
        LevelData.Init();
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