using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.ScriptableObjects.Tiles {
    [CreateAssetMenu(menuName = "Data/Tiles/Decoration")]
    public abstract class TileDecorationData : ScriptableObject {
        public Mesh Mesh;
        public abstract bool IsWalkable(CardinalDirection direction);
        public CardinalDirection Orientation;
        public Vector3 RelativePosition;
    }
}