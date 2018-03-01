using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.DataStructures {
    public class Map<T> : IReadOnlyCollection<MapEntry<T>> {
        private readonly T[,] _map;

        public int Width { get; }
        public int Height { get; }

        public int Count => Width * Height;

        public Map(int width, int height) {
            Width = width;
            Height = height;
            _map = new T[width, height];
        }

        public T this[int x, int y] {
            get { return this._map[x, y]; }
            set { this._map[x, y] = value; }
        }

        public IEnumerator<MapEntry<T>> GetEnumerator() {
            for (int y = 0; y < this.Height; y++) {
                for (int x = 0; x < this.Width; x++) {
                    yield return new MapEntry<T>(this, x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }
}