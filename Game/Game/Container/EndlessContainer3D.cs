using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Game.Game.Container
{
    public class EndlessContainer3D<T> : IContainer3D<T>
    {
        private Dictionary<Vector3i, T> _data = new();
        
        public bool Exists(Vector3i position)
        {
            return _data.ContainsKey(position);
        }
        
        public bool TryGet(Vector3i position, out T value)
        {
            return _data.TryGetValue(position, out value);
        }

        public T this[Vector3i position]
        {
            get => _data[position];
            set => _data[position] = value;
        }

        public bool InBounds(Vector3i position)
        {
            return true;
        }

        public IEnumerable<ICursor3D<T>> GetAll()
        {
            Cursor cursor = new Cursor(this);
            foreach (var dataValue in _data)
            {
                cursor.Position = dataValue.Key;
                cursor.Value = dataValue.Value;
                yield return cursor;
            }
        }

        public IEnumerable<ICursor3D<T>> GetRegion(Vector3i start, Vector3i size)
        {
            Vector3i end = start + size;
            
            Cursor cursor = new Cursor(this);
            foreach (var dataValue in _data)
            {
                cursor.Position = dataValue.Key;
                if (cursor.Position.X >= start.X && cursor.Position.X < end.X && cursor.Position.Y >= start.Y && cursor.Position.Y < end.Y && cursor.Position.Z >= start.Z && cursor.Position.Z < end.Z)
                {
                    cursor.Value = dataValue.Value;
                    yield return cursor;
                }
            }
        }
        
        public class Cursor : ICursor3D<T>
        {
            public EndlessContainer3D<T> _container;
            public Vector3i Position { get; set; }
            public T Value { get; set; }

            public Cursor(EndlessContainer3D<T> container)
            {
                _container = container;
            }

            public T Relative(Vector3i direction, in T _default)
            {
                Vector3i pos = Position + direction;
                return _container[pos];
            }
        }
    }
}