using System;
using System.Collections;
using Assets.Data.Grids;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.Grid.DataStructure;
using UnityEngine;

namespace Assets.Data.Command {
    [CreateAssetMenu(fileName = "MoveCommand", menuName = "Data/Commands/MoveCommand")]
    [System.Serializable]
    public class MoveCommand : BaseCommand
    { 
        /// <summary>
        ///     Move the player 1 step forward.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override IEnumerator Execute(MonoBehaviour coroutineRunner, LevelData level, Scripts.Player player, SequenceCycle cycle)
        {
            // Can i move forward? if not: return
            GridCell destination;

            bool SuccesfulMove = level.TryMoveInDirection(player, player.ViewDirection, out destination, cycle) && destination.IsValid;

            // Get WorldPosition from destination-GridCell
            GridMapData gridMap = level.GridMapData;
            Vector3 destinationPosition = GridHelper.GridToWorldPosition(gridMap, destination.XY);
            destinationPosition.y = player.transform.position.y;

            if (SuccesfulMove)
            {
                player.OnMoveTo?.Invoke(destinationPosition);
            }
            else
            {
                player.OnFailMoveTo?.Invoke(destinationPosition);
                yield break;

            }

            
        }

        public override string ToString()
        {
            return this.GetType() + ":" + base.ToString();
        }
    }
}