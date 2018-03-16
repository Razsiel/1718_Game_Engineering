using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class TurnCommand : ScriptableObject, ICommand
{
    public Sprite Icon { get; set; }

    [Header("TurnAngle: 90 for RightTurn, -90 for LeftTurn")]
    public int angle = 90;

    /// <summary>
    ///     Rotates the player 90 degrees
    ///     Left or Right based on te 'angle' field in TurnCommandData
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public IEnumerator Execute(Player player)
    {
        player.transform.Rotate(0, angle, 0);

        yield break;
    }
}
