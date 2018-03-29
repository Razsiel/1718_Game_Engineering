using UnityEngine;
using Assets.ScriptableObjects.Levels;

class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Player playerA;
    public Player playerB;
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
        levelData.Init();
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
