using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Data.Goal
{
    public class LocationGoal : LevelGoal {
        public List<KeyValuePair<Scripts.Player, Vector2Int>> TargetGridPosition;

        public override bool HasBeenReached(IEnumerable<Scripts.Player> players) {
            return TargetGridPosition.All(kvp => players.Single(p => p == kvp.Key).GridPosition == kvp.Value);
        }
    }
}
