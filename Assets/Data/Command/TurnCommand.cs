using UnityEngine;
using System.Collections;
using Assets.Data.Command;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Lib.Extensions;

[CreateAssetMenu(fileName = "TurnCommand", menuName = "Data/Commands/TurnCommand")]
[System.Serializable]
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
        player.ViewDirection = (CardinalDirection)MathHelper.Mod((int)player.ViewDirection + directionShiftOffset, 8);

        Vector3 targetEuler = player.ViewDirection.ToEuler();
        player.OnTurn?.Invoke(targetEuler);


        // TODO: THIS STUFF CAN BE REMOVED IF EVERYTHING WORKS
//        Quaternion targetRotation = Quaternion.Euler(targetEuler.x, targetEuler.y, targetEuler.z);
        

//        while (!player.transform.rotation.AlmostEquals(targetRotation, player.Data.MovementData.OffsetAlmostRotation))
//        {
//            player.transform.rotation = Quaternion.Lerp(
//                player.transform.rotation,
//                targetRotation,
//                player.Data.MovementData.RotationSpeed * Time.deltaTime);
//
//            yield return new WaitForEndOfFrame();
//        }
//
//        player.transform.rotation = targetRotation;
        yield break;
    }

    public override string ToString()
    {
        return this.GetType() + ":" + base.ToString();
    }
}