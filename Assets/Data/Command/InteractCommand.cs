using System.Collections;
using System.Collections.Generic;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.Behaviours;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Command {
    [CreateAssetMenu(fileName = "InteractCommand", menuName = "Data/Commands/InteractCommand")]
    [System.Serializable]
    public class InteractCommand : BaseCommand
    {
        public override IEnumerator Execute(MonoBehaviour coroutineRunner, LevelData level, Scripts.Player player) {
            IEnumerable<DecorationConfiguration> decorationsToAnimate;
            if (level.TryInteract(player, player.ViewDirection, out decorationsToAnimate))
            {
                player.OnInteract?.Invoke();
            }

            yield break;
        }

        public override string ToString()
        {
            return this.GetType() + ":" + base.ToString();
        }
    }
}