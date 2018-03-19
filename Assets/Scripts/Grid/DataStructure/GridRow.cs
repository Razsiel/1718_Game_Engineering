using System;
using Assets.ScriptableObjects.Tiles;
using UnityEngine;

namespace Assets.Scripts.Grid.DataStructure
{
    [Serializable]
    public class GridRow {
        [SerializeField]
        private TileData[] _tiles;

        public GridRow(int size) {
            _tiles = new TileData[size];
        }

        public TileData this[int index] {
            get { return _tiles[index]; }
            set { _tiles[index] = value; }
        }
    }
}