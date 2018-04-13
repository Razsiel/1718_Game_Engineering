using Assets.Data.Grids;
using Assets.Data.Levels;
using UnityEngine;

public class GridHelper {
    public static Vector3 GridToWorldPosition(LevelData levelData, Vector2Int gridPosition) {
        var grid = levelData.GridMapData;
        return GridToWorldPosition(grid, gridPosition);
    }

    public static Vector3 GridToWorldPosition(GridMapData grid, Vector2Int gridPosition) {
        var worldPos = new Vector3(gridPosition.x - (grid.Width - 1) * 0.5f,
                                   0,
                                   gridPosition.y - (grid.Height - 1) * 0.5f);
        return worldPos;
    }
}