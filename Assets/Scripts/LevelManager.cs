using System.Collections;
using System.Collections.Generic;
using Assets.Data.Levels;
using UnityEngine;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Photon;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;

    //List of players in the level
    public List<TGEPlayer> Players;

    //Level that is being played or about to
    public LevelData LevelData;

    public static LevelManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            Players = new List<TGEPlayer>();
        }
        else
            Destroy(this.gameObject);
    }

    //Start the level with given single player
    public void StartSinglePlayerGame(TGEPlayer player /*TODO , LevelData level*/)
    {
        //TO:DO Start the level given with the local player
        Players.Add(player);
        StartGame();
    }

    //Start the level with given multiplayer players
    public void StartMultiplayerGame(List<TGEPlayer> players /*TODO , LevelData level*/)
    {
        PhotonManager.Instance.GetOtherPlayers();
        Players = players;
        StartCoroutine(WaitForOtherPlayers());
        Players = players;
        StartGame();      
    }

    //Place for some generic logic to start the level
    private void StartGame(/*LevelData level*/)
    {
        //FOR LATER: this.LevelData = level;
        LevelData.Init(Players);
    }

    public IEnumerator WaitForOtherPlayers()
    {
        yield return new WaitUntil(() => Players[1].player != null);
    }

}
