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
        Vector3 destination = player.transform.position + (player.transform.forward * player.data.StepSize);
        float offset = Vector3.Distance(player.transform.position, destination);

        while (offset > player.data.OffsetTolerance)
        {
            offset = Vector3.Distance(player.transform.position, destination);
            player.transform.position = Vector3.Lerp(player.transform.position, destination, player.data.MovementSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        player.transform.position = destination;
        yield break;
    }
}
