namespace Assets.Scripts.DataStructures {
    public struct MapEntry<T> {

        private readonly Map<T> _map;

        public int X { get; }
        public int Y { get; }

        public MapEntry(Map<T> map, int y, int x) : this() {
            this._map = map;
            this.Y = y;
            this.X = x;
        }

        public T Value => this._map[this.X, this.Y];
    }
}
