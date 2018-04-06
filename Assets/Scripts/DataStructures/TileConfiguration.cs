using System;
using System.Linq;
using Assets.Data.Tiles;
using UnityEngine;

namespace Assets.Scripts.DataStructures {
    [Serializable]
    public class TileConfiguration {
        [SerializeField] public TileData Tile;
        [SerializeField] public TileDecorationData[] Decorations;
        
        public virtual bool IsWalkable(CardinalDirection direction)
        {
            return Decorations.Length == 0 || Decorations.All(d => d.IsWalkable(direction));
        }

        public virtual bool CanExit(CardinalDirection direction)
        {
            var oppositeDirection = direction.ToOppositeDirection();
            return IsWalkable(oppositeDirection);
        }

    }
}