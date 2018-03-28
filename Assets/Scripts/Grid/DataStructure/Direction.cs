namespace Assets.Scripts.DataStructures
{
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

    public static class DirectionExtensions
    {
        public static Direction ConvertToDirection(this CardinalDirection direction)
        {
            return (Direction)direction;

            // right turn: Player.Facing += 2 % 8;
        }

        public static CardinalDirection ConvertToCardinal(this Direction direction)
        {
            return (CardinalDirection)direction;
        }
    }
}