using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MoveCommand", menuName = "Data/Commands/MoveCommand")]
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
        if (!GameManager.GetInstance().LevelData.TryMoveInDirection(player.Data, player.ViewDirection))
            yield break;
            
        // Visual movement
        float stepDistance = player.Data.MovementData.StepSize;// * GameManager.GetInstance().LevelData.TileScale;
        Vector3 destination = player.transform.position + (player.transform.forward * stepDistance);
        float offset = Vector3.Distance(player.transform.position, destination);

        while (offset > player.Data.MovementData.OffsetTolerance)
        {
            offset = Vector3.Distance(player.transform.position, destination);
            player.transform.position = Vector3.Lerp(player.transform.position, destination,
                player.Data.MovementData.MovementSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        player.transform.position = destination;
        yield break;
    }
}