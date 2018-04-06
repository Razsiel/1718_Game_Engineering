using UnityEngine;
using System.Collections.Generic;
using Assets.Data.Levels;
using Assets.Scripts.Photon;
using Assets.ScriptableObjects;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    // Use PrefabContainer to access project-files
    public PrefabContainer PrefabContainer;

    public GameObject PlayerPrefab;
    public List<TGEPlayer> Players;
    
    public CommandLibrary CommandLibrary;

    //To be filled with the level (from UI layer)
    //For now the editor can handle this for us
    //To Be Added: [HideInInspector]
    public LevelData LevelData;

    #region GLOBAL_EVENTS

    public UnityAction<Player> PlayerInitialized;

    #endregion

    void Awake() {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start() {
        // Setup level
        Players = new List<TGEPlayer>();
        TGEPlayer player = new TGEPlayer();
        Players.Add(player);
        StartSinglePlayerGame(player);
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

            PlayerInitialized(playerComponent);
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
}