using System.Collections.Generic;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace Game.Game.Container
{
    public class SimpleContainer3D<T> : IContainer3D<T>
    {
        public Vector3i Size { get; }
        private T[] _data;

        public SimpleContainer3D(Vector3i size)
        {
            Debug.Assert(size.X >= 0 && size.Y >= 0 && size.Z >= 0, $"Size must be positive integer ({size})");
            Size = size;
            _data = new T[size.X * size.Y * size.Z];
        }

        public T this[Vector3i position]
        {
            get => _data[Index(position)];
            set => _data[Index(position)] = value;
        }
        
        public T this[int i]
        {
            get => _data[i];
            set => _data[i] = value;
        }

        private int Index(Vector3i position)
        {
            Debug.Assert(position.X >= 0 && position.X < Size.X && position.Y >= 0 && position.Y < Size.Y && position.Z >= 0 && position.Z < Size.Z, $"Position out of bounds {position}");
            return position.Z * Size.X * Size.Y + position.Y * Size.X + position.X;
        }
        
        private Vector3i Position(int index)
        {
            Vector3i position = new Vector3i();
            position.Z = index / (Size.X * Size.Y);
            index -= (position.Z * Size.X * Size.Y);
            position.Y = index / Size.X;
            position.X = index % Size.X;
            return position;
        }

        public bool InBounds(Vector3i position)
        {
            return position.X >= 0 && position.X < Size.X && position.Y >= 0 && position.Y < Size.Y && position.Z >= 0 && position.Z < Size.Z;
        }

        public IEnumerable<ICursor3D<T>> GetAll()
        {
            Cursor cursor = new Cursor(this);
            for (int i = 0; i < _data.Length; ++i)
            {
                cursor.Index = i;
                yield return cursor;
            }
        }

        public IEnumerable<ICursor3D<T>> GetRegion(Vector3i start, Vector3i size)
        {
            Debug.Assert(InBounds(start), $"Start position out of bounds {start}");
            Vector3i end = start + size;
            Debug.Assert(size.X >= 0 && end.X < Size.X && size.Y >= 0 && end.Y < Size.Y && size.Z >= 0 && end.Z < Size.Z , $"Size too big {size}");

            Cursor cursor = new Cursor(this);
            for (int x = start.X; x < end.X; ++x)
            {
                for (int y = start.Y; y < end.Y; ++y)
                {
                    for (int z = start.Z; z < end.Z; ++z)
                    {
                        cursor.Index = Index(new Vector3i(x, y, z));
                        yield return cursor;
                    }
                }
            }
        }

        public class Cursor : ICursor3D<T>
        {
            private SimpleContainer3D<T> _container;
            public int Index;
            public Vector3i? _Position;
            public Vector3i Position => _Position ?? _container.Position(Index);
            
            public T Value
            {
                get => _container[Index];
                set => _container[Index] = value;
            }

            public Cursor(SimpleContainer3D<T> container)
            {
                _container = container;
            }

            public T Neighbour(Vector3i direction, in T _default)
            {
                Debug.Assert(System.Math.Abs(direction.X) <= 1 && System.Math.Abs(direction.Y) <= 1 && System.Math.Abs(direction.Z) <= 1, $"Direction must be normalized ({direction})");
                Vector3i pos = Position + direction;
                return _container.InBounds(pos) ? _container[pos] : _default;
            }
        }
    }
}