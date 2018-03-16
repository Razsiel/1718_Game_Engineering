using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class WaitCommand : ScriptableObject, ICommand
{
    public Sprite Icon { get; set; }

    /// <summary>
    ///     Allows the player to skip 1 turn
    ///     The player does nothing
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public IEnumerator Execute(Player player)
    {
        //WaitAnimation?

        yield break;
    }
}
