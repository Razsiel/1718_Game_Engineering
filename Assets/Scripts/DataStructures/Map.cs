using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.DataStructures {
    public class Map<T> : IReadOnlyCollection<MapEntry<T>> {
        private readonly T[,] _map;

        /// <summary>
        /// Creates a map with a given width and height.
        /// </summary>
        /// <param name="width">The width of the map</param>
        /// <param name="height">The height of the map</param>
        public Map(int width, int height) {
            Width = width;
            Height = height;
            _map = new T[width, height];
        }

        /// <summary>
        /// Gets the width of the map.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the map.
        /// </summary>
        public int Height { get; }

        /// <inheritdoc />
        public int Count => Width * Height;

        /// <summary>
        /// Gets or sets the value of a map entry by its coordinates.
        /// </summary>
        /// <param name="x">The x position in the map</param>
        /// <param name="y">The y position in the map</param>
        public T this[int x, int y] {
            get { return this._map[x, y]; }
            set { this._map[x, y] = value; }
        }

        /// <summary>
        /// Returns the value of a map entry at a given entry.
        /// </summary>
        /// <param name="entry">The entry containing the x and y position in the map</param>
        public T this[MapEntry<T> entry] {
            get { return this[entry.X, entry.Y]; }
            set { this[entry.X, entry.Y] = value; }
        }

        /// <summary>
        /// Checks whether a specific entry location is within the map bounds.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsValidTile(int x, int y) {
            return x >= 0 && x < Width
                   && y >= 0 && y < Height;
        }

        /// <summary>
        /// Checks whether a given map entry is within the map bounds.
        /// </summary>
        /// <param name="entry">The map entry to check</param>
        public bool IsValidTile(MapEntry<T> entry)
        {
            return IsValidTile(entry.X, entry.Y);
        }

        #region IEnumerable implementation

        /// <inheritdoc />
        public IEnumerator<MapEntry<T>> GetEnumerator() {
            for (int y = 0; y < this.Height; y++) {
                for (int x = 0; x < this.Width; x++) {
                    yield return new MapEntry<T>(this, x, y);
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