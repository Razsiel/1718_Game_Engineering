using System.Collections;
using System.Collections.Generic;
using Assets.Data.Grids;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    void Awake()
    {
        EventManager.InitializeUi += Initialize;
        EventManager.MonologueEnded += LevelRect;
        EventManager.MonologueStart += MonologueRect;
    }

    void Initialize ()
    {
        _camera = gameObject.GetComponent<Camera>();
        AutoZoomCamera();
    }

    /// <summary>
    /// Sets the OrthographicSize of the camera depending on the grid-size of the level. 
    /// </summary>
    void AutoZoomCamera()
    {
        GridMapData gridMap = GameManager.GetInstance().LevelData.GridMapData;

        float levelSize = Mathf.Max(gridMap.Height, gridMap.Width);
        float aspectRatio = (float)Screen.height / Screen.width;

        // Camera should reduce the zoom-out by 0.2 per levelSize
        _camera.orthographicSize = 1 + (levelSize * aspectRatio) - (levelSize * 0.2f);
    }

    void LevelRect()
    {
        Rect r = new Rect(0f, 0.25f, 0.8f, 0.75f);
        _camera.DORect(r, 1f);
    }

    void MonologueRect(Monologue m)
    {
        Rect r = new Rect(0f, 0.25f, 1f, 0.75f);
        _camera.DORect(r, 1f);
    }

    void FullRect()
    {
        Rect r = new Rect(0f, 0f, 1f, 1f);
        _camera.DORect(r, 1f);
    }
}
