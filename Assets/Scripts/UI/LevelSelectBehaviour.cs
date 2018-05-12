﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.Photon.LevelSelect;
using UnityEngine;
using UnityEngine.Assertions;
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
    private LevelSelectPhotonManager LevelSelectPhotonManager;

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
            this.LevelSelectPhotonManager = LevelSelectPhotonManagerGO.GetComponent<LevelSelectPhotonManager>();
            LevelSelectPhotonManager.Init(PlayButton, LevelScene, _gameInfo);
        }

        LevelScroller.OnSelectionPageChangedEvent.AddListener(page =>
        {
            print($"Changed pagenr to #{page}");
            _selectedLevel = levels[page];
            UpdatePreviewImageColors(page);

        });
    }

    private void UpdatePreviewImageColors(int selectedLevel)
    {
        for (int i = 0; i < LevelScroller.ChildObjects.Length; i++)
        {
            LevelScroller.ChildObjects[i].transform.GetChild(2).GetChild(1).GetComponent<Image>().color =
                new Color32(0x74, 0x41, 0x41, 0xFF);
        }

        LevelScroller.ChildObjects[selectedLevel].transform.GetChild(2).GetChild(1).GetComponent<Image>().color =
            new Color(255f, 255f, 255f, 255f);
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
            LevelSelectPhotonManager.StartLevel(_selectedLevel);
        }
        else
            SceneManager.LoadScene(LevelScene);
    }
}