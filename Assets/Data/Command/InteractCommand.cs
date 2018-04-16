using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Behaviours;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Command {
    [CreateAssetMenu(fileName = "InteractCommand", menuName = "Data/Commands/InteractCommand")]
    [System.Serializable]
    public class InteractCommand : BaseCommand
    {
        public override IEnumerator Execute(Scripts.Player player) {
            IEnumerable<DecorationConfiguration> decorationsToAnimate;
            if (GameManager.GetInstance().LevelData.TryInteract(player, player.ViewDirection, out decorationsToAnimate))
            {
                Debug.Log("Interacting...");
                // animate character
            }

            yield break;
        }

        public override string ToString()
        {
            return this.GetType() + ":" + base.ToString();
        }
    }
}