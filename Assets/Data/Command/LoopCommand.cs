using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Levels;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Data.Command
{
    [CreateAssetMenu(fileName = "LoopCommand", menuName = "Data/Commands/LoopCommand")]
    [System.Serializable]
    public class LoopCommand : BaseCommand {
        public Sequence Sequence;
        [HideInInspector]
        public int LoopCount;

        public override IEnumerator Execute(MonoBehaviour coroutineRunner, LevelData level, Scripts.Player player) {
            for (int i = 0; i < LoopCount; i++) {
                yield return Sequence?.Run(player, level, player);
            }
        }
    }
}
