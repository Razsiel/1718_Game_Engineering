using UnityEngine;
using System.Collections;
using Assets.Data.Grids;
using Assets.Scripts;
using Assets.Scripts.Grid.DataStructure;

[CreateAssetMenu(fileName = "MoveCommand", menuName = "Data/Commands/MoveCommand")]
[System.Serializable]
public class MoveCommand : BaseCommand
{
    /// <summary>
    ///     Move the player 1 step forward.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public override IEnumerator Execute(Player player)
    {
        // Can i move forward? if not: return
        GridCell destination;
        if (!GameManager.GetInstance().LevelData.TryMoveInDirection(player, player.ViewDirection, out destination)
            || !destination.IsValid)
            yield break;

        // Get WorldPosition from destination-GridCell
        GridMapData gridMap = GameManager.GetInstance().LevelData.GridMapData;
        Vector3 destinationPosition = GridHelper.GridToWorldPosition(gridMap, destination.XY);
        destinationPosition.y = player.transform.position.y;

        // Visual movement
        while (!player.transform.position.AlmostEquals(destinationPosition, player.Data.MovementData.OffsetAlmostPosition))
        {
            player.transform.position = Vector3.Lerp(
                player.transform.position, 
                destinationPosition,
                player.Data.MovementData.MovementSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        player.transform.position = destinationPosition;
    }
}