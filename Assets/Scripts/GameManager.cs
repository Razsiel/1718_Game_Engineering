using UnityEngine;
using System.Collections.Generic;
using Assets.Data.Levels;
using Assets.Scripts.Photon;
using Assets.ScriptableObjects;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    // Use PrefabContainer to access project-files
    public PrefabContainer PrefabContainer;
    
    public List<TGEPlayer> Players;
    
    public CommandLibrary CommandLibrary;

    //To be filled with the level (from UI layer)
    //For now the editor can handle this for us
    //To Be Added: [HideInInspector]
    public LevelData LevelData;

    void Awake() {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start() {
        // Temp Players setup
        Players = new List<TGEPlayer>();
        TGEPlayer player = new TGEPlayer();
        Players.Add(player);

        // Start Level Events
        // Make sure de scene contains required GameObjects

        // PrefabContainer filled and available
        AssertAllNotNull();

        // Initialize LevelData with Players
        StartSinglePlayerGame(player);

        // Initialize UI
        EventManager.OnInitializeUi();

        // Link Player2 to Photon instance
        // Start Level
    }

    public void StartSinglePlayerGame(TGEPlayer player /*, LevelData level*/) {
        //TO:DO Start the level given with the local player
        var players = new List<TGEPlayer>() {player};
        CreatePlayers(players);
        LevelData.Init(players);
        LevelPresenter.Present(LevelData, players);
    }

    private void CreatePlayers(List<TGEPlayer> players) {
        for (int i = 0; i < players.Count; i++) {
            var playerObject = Instantiate(this.PrefabContainer.PlayerPrefab, Vector3.zero, Quaternion.identity, this.transform);
            var playerComponent = playerObject.GetComponent<Player>();
            playerComponent.PlayerNumber = i;
            players[i].PlayerObject = playerObject;
            players[i].player = playerComponent;

//            PlayerInitialized(playerComponent);
        }
    }

    public void StartMultiplayerGame(List<TGEPlayer> players /*, LevelData level*/) {
        Players = players;

        ////Lets do some GameStarting logic here
        PhotonManager.Instance.StartMultiplayerGame(LevelData, Players);
    }

    public static GameManager GetInstance() {
        return _instance;
    }

    private bool AssertAllNotNull()
    {
        Assert.IsNotNull(PrefabContainer);
        Assert.IsNotNull(PrefabContainer.PlayerPrefab);
//        Assert.IsNotNull(PrefabContainer.ImageNotFound);
        Assert.IsNotNull(CommandLibrary);
        Assert.IsNotNull(CommandLibrary.MoveCommand);
        Assert.IsNotNull(CommandLibrary.TurnRightCommand);
        Assert.IsNotNull(CommandLibrary.TurnLeftCommand);
        Assert.IsNotNull(CommandLibrary.WaitCommand);
        Assert.IsNotNull(CommandLibrary.InteractCommand);

        return true;
    }
}
