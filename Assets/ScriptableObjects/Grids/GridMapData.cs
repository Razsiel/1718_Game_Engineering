using System;
using System.Collections;
using System.Collections.Generic;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Grid.DataStructure;
using UnityEngine;

namespace Assets.ScriptableObjects.Grids
{
    [Serializable]
    [CreateAssetMenu(fileName = "Default Grid", menuName = "Data/Grid")]
    public class GridMapData : ScriptableObject, IReadOnlyCollection<GridCell> {
        [SerializeField]
        public TileData DefaultTile;

        [SerializeField]
        private GridRow[] _map;
        private int _width;
        private int _height;

        /// <summary>
        /// Creates a mapData with a given width and height.
        /// </summary>
        /// <param name="width">The width of the mapData</param>
        /// <param name="height">The height of the mapData</param>
        public GridMapData(int width, int height) {
            Width = width;
            Height = height;
            RecalculateGrid();
        }

        /// <summary>
        /// Gets the width of the mapData.
        /// </summary>
        [ExposeProperty]
        public int Width {
            get { return _width; }
            set {
                _width = value;
                RecalculateGrid();
            }
        }

        /// <summary>
        /// Gets the height of the mapData.
        /// </summary>
        [ExposeProperty]
        public int Height {
            get { return _height; }
            set {
                _height = value;
                RecalculateGrid();
            }
        }

        /// <inheritdoc />
        public int Count => Width * Height;

        /// <summary>
        /// Gets or sets the value of a mapData entry by its coordinates.
        /// </summary>
        /// <param name="x">The x position in the mapData</param>
        /// <param name="y">The y position in the mapData</param>
        public TileData this[int x, int y] {
            get { return this._map[x][y]; }
            set { this._map[x][y] = value; }
        }

        /// <summary>
        /// Returns the value of a mapData entry at a given entry.
        /// </summary>
        /// <param name="entry">The entry containing the x and y position in the mapData</param>
        public TileData this[GridCell entry] {
            get { return this[entry.X, entry.Y]; }
            set { this[entry.X, entry.Y] = value; }
        }

        /// <summary>
        /// Checks whether a specific entry location is within the mapData bounds.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsValidTile(int x, int y) {
            return x >= 0 && x < Width
                   && y >= 0 && y < Height;
        }

        /// <summary>
        /// Checks whether a given mapData entry is within the mapData bounds.
        /// </summary>
        /// <param name="entry">The mapData entry to check</param>
        public bool IsValidTile(GridCell entry) {
            return IsValidTile(entry.X, entry.Y);
        }


        /// <summary>
        /// Recreates the mapData according to the mapData's width and height. Preserves previous data, but resizing smaller will cause 'out of bounds' data to be lost!
        /// </summary>
        public void RecalculateGrid()
        {
            var old = _map;
            _map = new GridRow[Width];
            for (int i = 0; i < _map.Length; i++)
            {
                _map[i] = new GridRow(Height);
                for (int j = 0; j < Height; j++)
                {
                    if (old != null) {
                        _map[i][j] = old[i][j]; // copy old elements over
                    }
                    else {
                        _map[i][j] = DefaultTile;
                    }
                    
                }
            }
        }

        #region IEnumerable implementation

        /// <inheritdoc />
        public IEnumerator<GridCell> GetEnumerator() {
            for (int y = 0; y < this.Height; y++) {
                for (int x = 0; x < this.Width; x++) {
                    yield return new GridCell(this, x, y);
                }
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}