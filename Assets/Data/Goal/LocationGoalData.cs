using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Data.Goal
{
    [CreateAssetMenu(menuName = "Data/Level/Goals/Location", fileName = "LocationGoal")]
    public class LocationGoalData : LevelGoalData {
        [SerializeField]
        public List<LevelLocation> TargetGridPosition;
        
        public override bool HasBeenReached(IEnumerable<Scripts.Player> players) {
            return TargetGridPosition.All(levelLocation => players.Single(p => p.PlayerNumber == levelLocation.PlayerNumber).GridPosition == levelLocation.Location);
        }
    }

    [Serializable]
    public struct LevelLocation {
        [SerializeField] public int PlayerNumber;
        [SerializeField] public Vector2Int Location;
    }
}
