using UnityEngine;
using Assets.ScriptableObjects.Levels;
using System.Collections.Generic;
using Assets.Scripts.Photon;
using Assets.ScriptableObjects;
using Assets.ScriptableObjects.Player;
using Assets.Scripts.DataStructures;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    
    public GameObject PlayerPrefab;
    public List<TGEPlayer> Players;

    //public GameObject PlayerPrefab;
    public CommandLibrary CommandLibrary;

    //To be filled with the level (from UI layer)
    //For now the editor can handle this for us
    //To Be Added: [HideInInspector]
    public LevelData LevelData;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        Players = new List<TGEPlayer>();

        // Setup level
        TGEPlayer player = new TGEPlayer();
        GameObject playerClone = Instantiate(PlayerPrefab, null);
        player.player = (Player)playerClone.GetComponent("Player");

        Players.Add(player);
        StartSinglePlayerGame(player);
        LevelData.Init(Players);
    }

    void Start()
    {
        
    }

    public void StartSinglePlayerGame(TGEPlayer player /*, LevelData level*/)
    {
        //TO:DO Start the level given with the local player

        LevelData.Init(new List<TGEPlayer>() { player });
    }

    public void StartMultiplayerGame(List<TGEPlayer> players /*, LevelData level*/)
    {
        Players = players;

        ////Lets do some GameStarting logic here
        PhotonManager.Instance.StartMultiplayerGame(LevelData, Players);
    }

  

    public static GameManager GetInstance() {
        return _instance;
    }
}