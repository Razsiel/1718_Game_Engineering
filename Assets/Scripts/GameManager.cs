using UnityEngine;
using Assets.ScriptableObjects.Levels;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    //Temp scene players (testing purposes)
    public Player PlayerA;
    public Player PlayerB;

    public List<Player> Players = new List<Player>();
    public CommandLibrary CommandLibrary;
    public LevelData LevelData;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        // Start level
        // add players to players-list
        Players.Add(PlayerA);
        Players.Add(PlayerB);

        // Set players on start positions
        LevelData.Init();
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }
}