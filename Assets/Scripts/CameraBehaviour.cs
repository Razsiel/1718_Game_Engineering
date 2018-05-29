using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Data.Grids;
using Assets.Data.Levels;
using Assets.Scripts;
using DG.Tweening;
using RockVR.Video;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    void Awake() {
        EventManager.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded(GameInfo gameInfo) {
        EventManager.OnLevelLoaded -= OnLevelLoaded;
        _camera = gameObject.GetComponent<Camera>();
        AutoZoomCamera(gameInfo.Level);

        if (PlayerPrefs.GetInt("RecordVideos") == 1)
        {
            RecordVideo();
        }
    }

    /// <summary>
    /// Sets the OrthographicSize of the camera depending on the grid-size of the level. 
    /// </summary>
    /// <param name="levelData"></param>
    void AutoZoomCamera(LevelData levelData)
    {
        float levelSize = Mathf.Max(levelData.GridMapData.Height, levelData.GridMapData.Width);
        float aspectRatio = (float)Screen.height / Screen.width;

        // Camera should reduce the zoom-out by 0.2 per levelSize
        _camera.orthographicSize = 2.5f + (levelSize * aspectRatio) - (levelSize * 0.2f);
    }

    void RecordVideo()
    {
        var VideoCaptureComponent = GetComponent<VideoCapture>();
        
        VideoCaptureComponent.customPath = true;
        VideoCaptureComponent.customPathFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Progranimals\\Video";
        PathConfig.SaveFolder = VideoCaptureComponent.customPathFolder + "\\";

        VideoCaptureCtrl.instance.videoCaptures = new VideoCaptureBase[1] { VideoCaptureComponent };

        VideoCaptureCtrl.instance.StartCapture();
        EventManager.OnWinScreenContinueClicked += VideoCaptureCtrl.instance.StopCapture;
    }

    void OnDestroy()
    {
        VideoCaptureCtrl.instance?.StopCapture();
    }
}
