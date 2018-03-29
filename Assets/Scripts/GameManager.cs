using UnityEngine;
using Assets.ScriptableObjects.Levels;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    //Temp scene players (testing purposes)
    public Player playerA;
    public Player playerB;

    public GameObject PlayerPrefab;
    public List<Player> Players = new List<Player>();
    public CommandLibrary CommandLibrary;
    public LevelData LevelData;

    void Awake() {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        // Setup level
        LevelData.Init();
    }

    void Start() {

    }

    public static GameManager GetInstance() {
        return _instance;
    }
}