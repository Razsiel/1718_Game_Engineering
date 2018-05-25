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
        public int LoopCount = 1;

        public override IEnumerator Execute(MonoBehaviour coroutineRunner, LevelData level, Scripts.Player player, SequenceCycle cycle) {
//            Debug.Log($"Starting loop with {LoopCount} iterations");
//            for (int i = 1; i <= LoopCount; i++) {
//                Debug.Log($"Running iteration sequence: {i}/{LoopCount}");
//                yield return Sequence?.Run(player, level, player);
//            }
//            Debug.Log($"Finished Loop");
            yield break;
        }

        public override BaseCommand Init() {
            Sequence = new Sequence();
            LoopCount = 1;
            return this;
        }
    }
}
