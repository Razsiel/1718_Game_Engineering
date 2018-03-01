using System;
using System.Collections.Generic;

namespace Assets.Scripts.DataStructures {
    public struct MapEntry<T> : IEquatable<MapEntry<T>> {
        private readonly Map<T> _map;
        public int X { get; }
        public int Y { get; }

        /// <summary>
        /// Creates a new map entry reference
        /// </summary>
        /// <param name="map">The map it belongs to</param>
        /// <param name="y">The X position of the entry</param>
        /// <param name="x">The Y position of the entry</param>
        public MapEntry(Map<T> map, int y, int x) {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            this._map = map;
            this.Y = y;
            this.X = x;
        }

        /// <summary>
        /// Gets the object associated with this entry
        /// </summary>
        public T Value => this._map[this.X, this.Y];

        /// <summary>
        /// Returns true if the entry is within the map bounds
        /// </summary>
        public bool IsValid => this._map != null && this._map.IsValidTile(this);

        /// <summary>
        /// Returns all eight map-entries neighbouring this one, whether or not they are outside the map. 
        /// </summary>
        public IEnumerable<MapEntry<T>> PossibleNeighbours {
            get {
                // start at 1 since Direction.Unknown refers to the current tile after stepping
                for (int i = 1; i < 9; i++) {
                    yield return this.Neighbour((Direction) i);
                }
            }
        }

        /// <summary>
        /// Returns all map-entries neighbouring this one without going outside of the range of the map. 
        /// </summary>
        public IEnumerable<MapEntry<T>> ValidNeighbours {
            get {
                // start at 1 since Direction.Unknown refers to the current tile after stepping
                for (int i = 1; i < 9; i++) {
                    var entry = this.Neighbour((Direction) i);
                    if (entry.IsValid) {
                        yield return entry;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a map entry neighbouring this one in a given direction
        /// </summary>
        public MapEntry<T> Neighbour(Direction direction) {
            return this.Neighbour(direction.ToStep());
        }

        /// <summary>
        /// Internal method for converting a direction step to a map entry
        /// </summary>
        private MapEntry<T> Neighbour(Step step) {
            return new MapEntry<T>(this._map,
                                   this.X + step.X,
                                   this.Y + step.Y);
        }

        #region IEquatable implementation

        /// <inheritdoc />
        public bool Equals(MapEntry<T> other) => _map == other._map && X == other.X && Y == other.Y;

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is MapEntry<T> && Equals((MapEntry<T>) obj);
        }

        /// <summary>
        /// Returns the hashcode for this instance
        /// </summary>
        public override int GetHashCode() {
            unchecked {
                var hashCode = (_map != null ? _map.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.X;
                hashCode = (hashCode * 397) ^ this.Y;
                return hashCode;
            }
        }

        /// <summary>
        /// Indicates whether this instance references the same entry as another one.
        /// </summary>
        public static bool operator ==(MapEntry<T> entry1, MapEntry<T> entry2) {
            return entry1.Equals(entry2);
        }

        /// <summary>
        /// Indicates whether this instance references a different entry than another one.
        /// </summary>
        public static bool operator !=(MapEntry<T> entry1, MapEntry<T> entry2) {
            return !(entry1 == entry2);
        }

        #endregion
    }

    #region Internal helpers

    /// <summary>
    /// Internal extension methods
    /// </summary>
    internal static class Extensions {
        /// <summary>
        /// Internal list of direction delta's. NOTE: If the order of the Direction enums changes, the order of the items need to reflect the changes made.
        /// </summary>
        internal static readonly Step[] DirectionDeltas = {
            new Step(0, 0), // Unknown
            new Step(0, 1), // Up
            new Step(1, 1), // UpRight
            new Step(1, 0), // Right
            new Step(1, -1), // DownRight
            new Step(0, -1), // Down
            new Step(-1, -1), // DownLeft
            new Step(-1, 0), // Left
            new Step(1, -1), // UpLeft
        };

        /// <summary>
        /// Gets the predefined step value from an internal list of directions
        /// </summary>
        internal static Step ToStep(this Direction direction) {
            return DirectionDeltas[(int) direction];
        }
    }

    /// <summary>
    /// Internal structure used for simplifying a 
    /// </summary>
    internal struct Step {
        internal readonly sbyte X;
        internal readonly sbyte Y;

        internal Step(sbyte x, sbyte y) {
            X = x;
            Y = y;
        }
    }

    #endregion
}