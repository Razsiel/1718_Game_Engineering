using System.Collections;
using System.Collections.Generic;
using Assets.Data.Grids;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    // Use this for initialization
    void Start ()
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
}
