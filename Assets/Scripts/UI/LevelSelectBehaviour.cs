using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.Photon.LevelSelect;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;
using Utilities;
using UnityEngine.UI;

public class LevelSelectBehaviour : MonoBehaviour
{

    private GameInfo _gameInfo;
    private LevelData _selectedLevel;

    public SceneField LevelScene;
    public HorizontalScrollSnap LevelScroller;
    public GameObject LevelUIPrefab;
    public GameObject PlayButton;
    public GameObject LevelSelectPhotonManagerGO;
    private LevelSelectPhotonManager _levelSelectPhotonManager;

    public static UnityAction<int> SelectedLevelChanged;

    void Awake()
    {
        GlobalData.SceneDataLoader.OnSceneLoaded += gameInfo =>
        {
            this._gameInfo = gameInfo;
            Init();
        };
    }

    void Init()
    {
        var levels = _gameInfo.LevelLibrary.Levels;
        Assert.IsNotNull(levels);
        Assert.IsTrue(levels.Any());
        print(levels.Count);
        _selectedLevel = levels[0];
        LevelScroller.ChildObjects = new GameObject[levels.Count];
        for (var levelNumber = 0; levelNumber < levels.Count; levelNumber++)
        {
            var levelData = levels[levelNumber];
            var prefab = Instantiate(LevelUIPrefab);
            var levelPreviewBehaviour = prefab.GetComponent<LevelPreviewBehaviour>();
            Assert.IsNotNull(levelPreviewBehaviour);
            levelPreviewBehaviour.Init(1, levelNumber + 1, levelData);

            LevelScroller.AddChild(prefab);
        }

        if (_gameInfo.IsMultiplayer)
        {
            //if(_gameInfo.LocalPlayer.photonPlayer.IsMasterClient) 
            this._levelSelectPhotonManager = LevelSelectPhotonManagerGO.GetComponent<LevelSelectPhotonManager>();
            _levelSelectPhotonManager.Init(PlayButton, LevelScene, _gameInfo, LevelScroller);
        }

        LevelScroller.OnSelectionPageChangedEvent.AddListener(page =>
        {
            print($"Changed pagenr to #{page}");
            _selectedLevel = levels[page];
        });
    }

    public void OnPlayClick()
    {
        print("Clicked PLAY!");
        Assert.IsNotNull(_selectedLevel);
        _gameInfo.Level = _selectedLevel;
        print($"We're gonna play level: {_selectedLevel.Name}");

        if (_gameInfo.IsMultiplayer)
        {
            print("Multiplayer: We are gonna start the selected level");
            _levelSelectPhotonManager.StartLevel(_selectedLevel);
        }
        else
            SceneManager.LoadScene(LevelScene);
    }
}