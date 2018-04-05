using System;
using Assets.Data.Tiles;
using UnityEngine;

namespace Assets.Scripts.Grid.DataStructure
{
    [Serializable]
    public class GridRow {
        [SerializeField]
        private TileData[] _tiles;

        public GridRow(int size, TileData defaultTileData = null) {
            _tiles = new TileData[size];
            if (defaultTileData != null) {
                for (int i = 0; i < size; i++) {
                    _tiles[i] = defaultTileData;
                }
            }
        }

        public TileData this[int index] {
            get { return _tiles[index]; }
            set { _tiles[index] = value; }
        }

        public int Length => _tiles.Length;

        public void Resize(int newSize) {
            Array.Resize(ref _tiles, newSize);
        }
    }
}