using Assets.ScriptableObjects.Levels;
using UnityEngine;

public class GridHelper {
    public static Vector3 GridToWorldPosition(LevelData levelData, Vector2Int gridPosition) {
        var grid = levelData.GridMapData;
        var worldPos = new Vector3(gridPosition.x - (grid.Width - 1) * 0.5f,
                                   0,
                                   gridPosition.y - (grid.Height - 1) * 0.5f);
        return worldPos;
    }
}