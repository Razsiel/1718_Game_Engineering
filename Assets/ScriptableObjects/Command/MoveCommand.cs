using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class MoveCommand : ScriptableObject, ICommand
{
    public Sprite Icon { get; set; }

    /// <summary>
    ///     Move the player 1 step forward.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public IEnumerator Execute(Player player)
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
