using System;
using Assets.Data.Tiles;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.Grid.DataStructure
{
    [Serializable]
    public class GridRow {
        [SerializeField]
        private TileConfiguration[] _tiles;

        public GridRow(int size, TileConfiguration defaultTileConfiguration = null) {
            _tiles = new TileConfiguration[size];
            if (defaultTileConfiguration != null) {
                for (int i = 0; i < size; i++) {
                    _tiles[i] = defaultTileConfiguration;
                }
            }
        }

        public TileConfiguration this[int index] {
            get { return _tiles[index]; }
            set { _tiles[index] = value; }
        }

        public int Length => _tiles.Length;

        public void Resize(int newSize) {
            Array.Resize(ref _tiles, newSize);
        }
    }
}