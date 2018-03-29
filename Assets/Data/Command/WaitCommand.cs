using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WaitCommand", menuName = "Data/Commands/WaitCommand")]
public class WaitCommand : BaseCommand
{
    /// <summary>
    ///     Allows the player to skip 1 turn
    ///     The player does nothing
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public override IEnumerator Execute(Player player)
    {
        //WaitAnimation?

        yield break;
    }
}