using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

[CreateAssetMenu(fileName = "TurnCommand", menuName = "Data/Commands/TurnCommand")]
public class TurnCommand : BaseCommand
{
    [Header("TurnAngle: 90 for RightTurn, -90 for LeftTurn")]
    public int angle = 90;

    /// <summary>
    ///     Rotates the player 90 degrees
    ///     Left or Right based on te 'angle' field in TurnCommandData
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public override IEnumerator Execute(Player player)
    {
        // For swapping CardinalDirection: always 2 or -2 (90 / 45 = 2)
        int directionShiftOffset = angle / 45;
        player.ViewDirection = (CardinalDirection) (((int) player.ViewDirection + directionShiftOffset) % 8);

        player.transform.Rotate(0, angle, 0);

        yield break;
    }
}