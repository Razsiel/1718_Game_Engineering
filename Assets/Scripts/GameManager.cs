using UnityEngine;
using Assets.ScriptableObjects.Levels;
using System.Collections.Generic;

class GameManager : MonoBehaviour
{
    private static GameManager instance;

    //Temp scene players (testing purposes)
    public Player playerA;
    public Player playerB;

    public List<Player> players = new List<Player>();
    public CommandLibrary commandLibrary;
    public LevelData levelData;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
   
    void Start()
    {
        // Start level
        // add players to players-list
        players.Add(playerA);
        players.Add(playerB);

        // Set players on start positions
        levelData.InitPlayers();


    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
