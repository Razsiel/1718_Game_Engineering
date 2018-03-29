using System;
using UnityEngine;

namespace Assets.Scripts.DataStructures {
    public enum Direction : byte {
        Unknown = 0,
        Up = 1,
        UpRight = 2,
        Right = 3,
        DownRight = 4,
        Down = 5,
        DownLeft = 6,
        Left = 7,
        UpLeft = 8
    }

    public enum CardinalDirection : byte {
        North = 1,
        East = 3,
        South = 5,
        West = 7
    }

    public static class DirectionExtensions {
        public static Direction ToDirection(this CardinalDirection cardinal) {
            return (Direction) cardinal;

            // right turn: Player.Facing += 2 % 8;
        }

        public static CardinalDirection ToCardinalDirection(this Direction direction) {
            return (CardinalDirection) direction;
        }

        /// <summary>
        /// Converts a cardinal direction into a Vector2 where the coordinates are located within the XY-plane with a length of 1
        /// </summary>
        /// <param name="cardinal">The cardinal direction</param>
        /// <returns>Vector2 containing values of either -1, 0 or 1</returns>
        public static Vector2Int ToVector2(this CardinalDirection cardinal) {
            return ToVector2(cardinal.ToDirection());
        }

        /// <summary>
        /// Converts a direction into a Vector2 where the coordinates are located within the XY-plane with a length of 1
        /// </summary>
        /// <param name="direction">The direction</param>
        /// <returns>Vector2 containing values of either -1, 0 or 1</returns>
        public static Vector2Int ToVector2(this Direction direction) {
            switch (direction) {
                case Direction.Unknown:
                    return new Vector2Int(0, 0);
                case Direction.Up:
                    return new Vector2Int(0, 1);
                case Direction.UpRight:
                    return new Vector2Int(1, 1);
                case Direction.Right:
                    return new Vector2Int(1, 0);
                case Direction.DownRight:
                    return new Vector2Int(1, -1);
                case Direction.Down:
                    return new Vector2Int(0, -1);
                case Direction.DownLeft:
                    return new Vector2Int(-1, -1);
                case Direction.Left:
                    return new Vector2Int(-1, 0);
                case Direction.UpLeft:
                    return new Vector2Int(-1, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static CardinalDirection ToOppositeDirection(this CardinalDirection direction) {
            return (CardinalDirection) (((int)direction + 4) % 8);
        }
    }
}