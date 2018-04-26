﻿using System.Collections;
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
        public override IEnumerator Execute(LevelData level, Scripts.Player player)
        {
            // Can i move forward? if not: return
            GridCell destination;
            if (!level.TryMoveInDirection(player, player.ViewDirection, out destination)
                || !destination.IsValid)
                yield break;

            // Get WorldPosition from destination-GridCell
            GridMapData gridMap = level.GridMapData;
            Vector3 destinationPosition = GridHelper.GridToWorldPosition(gridMap, destination.XY);
            destinationPosition.y = player.transform.position.y;
            
            player.OnMoveTo?.Invoke(destinationPosition);
        }

        public override string ToString()
        {
            return this.GetType() + ":" + base.ToString();
        }
    }
}