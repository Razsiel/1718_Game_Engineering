using System.Collections;
using UnityEngine;

namespace Assets.Data.Command {
    [CreateAssetMenu(fileName = "WaitCommand", menuName = "Data/Commands/WaitCommand")]
    [System.Serializable]
    public class WaitCommand : BaseCommand
    {
        /// <summary>
        ///     Allows the player to skip 1 turn
        ///     The player does nothing
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override IEnumerator Execute(Scripts.Player player)
        {
            //WaitAnimation?

            yield break;
        }

        public override string ToString()
        {
            return this.GetType() + ":" + base.ToString();
        }
    }
}