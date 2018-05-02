using System.Collections;
using System.Collections.Generic;
using Assets.Data.Grids;
using Assets.Data.Levels;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    void Awake() {
        EventManager.OnLevelLoaded += (gameInfo) => {
            _camera = gameObject.GetComponent<Camera>();
            AutoZoomCamera(gameInfo.Level);
        };
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
        _camera.orthographicSize = 1 + (levelSize * aspectRatio) - (levelSize * 0.2f);
    }
}
