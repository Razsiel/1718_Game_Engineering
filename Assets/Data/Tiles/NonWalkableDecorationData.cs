using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Tiles
{
    [CreateAssetMenu(menuName = "Data/Tiles/NonWalkableDecoration")]
    public class NonWalkableDecorationData : TileDecorationData
    {
        public override bool IsWalkable(CardinalDirection direction) {
            return false;
        }
    }
}
