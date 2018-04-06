using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Data.Goal
{
    public class LocationGoal : LevelGoal {
        public Vector2Int TargetGridPosition;


        public override bool HasBeenReached(List<global::Assets.Scripts.Player> players) {
            throw new NotImplementedException();
        }
    }
}
