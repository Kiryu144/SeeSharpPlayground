using System.Collections.Generic;
using System.Diagnostics;
using OpenTK.Mathematics;

namespace Game.Game.Container
{
    public class LimitedContainer3D<T> : IContainer3D<T>
    {
        public readonly int SideLength;
        public int Size => _data.Length;

        private readonly int _bitsPerSideLength;
        private readonly int _twiceBitsPerSideLength;
        private readonly int _bitmask;
        private readonly T[] _data;

        public LimitedContainer3D(int sideLength)
        {
            Debug.Assert(sideLength > 0 && (sideLength & (sideLength - 1)) == 0, "Side length must be a positive power of two.");
            SideLength = sideLength;
            _bitsPerSideLength = (int) System.Math.Sqrt(sideLength);
            _twiceBitsPerSideLength = _bitsPerSideLength * 2;
            _bitmask = ((sideLength - 1) << 1) >> 1;
            _data = new T[SideLength * SideLength * SideLength];
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
        
        public int Index(Vector3i position)
        {
            Debug.Assert(position.X >= 0 && position.X < SideLength && position.Y >= 0 && position.Y < SideLength && position.Z >= 0 && position.Z < SideLength, $"Position out of bounds {position}");
            int index = position.Z << _twiceBitsPerSideLength;
            index |= position.Y << _bitsPerSideLength;
            index |= position.X;
            return index;
        }
        
        // TODO Static Caching
        public Vector3i Position(int i)
        {
            Debug.Assert(i >= 0 && i < _data.Length, $"Index out of bounds {i}");
            Vector3i position;
            position.X = i & _bitmask;
            position.Y = (i >> _bitsPerSideLength) & _bitmask;
            position.Z = (i >> _twiceBitsPerSideLength) & _bitmask;
            return position;
        }

        public bool InBounds(Vector3i position)
        {
            return position.X >= 0 && position.X < SideLength && position.Y >= 0 && position.Y < SideLength && position.Z >= 0 && position.Z < SideLength;
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
            if (size.X <= 0 || size.Y <= 0 || size.Z <= 0)
            {
                yield break;
            }
            
            Debug.Assert(InBounds(start), $"Start position out of bounds {start}");
            Vector3i end = start + size;
            Debug.Assert(end.X <= SideLength && end.Y <= SideLength && end.Z <= SideLength , $"Size too big {size}");
            
            Cursor cursor = new Cursor(this);
            for (int x = start.X; x < end.X; ++x)
            {
                for (int y = start.Y; y < end.Y; ++y)
                {
                    for (int z = start.Z; z < end.Z; ++z)
                    {
                        Vector3i pos = new Vector3i(x, y, z);
                        cursor._Position = pos;
                        cursor.Index = Index(pos);
                        yield return cursor;
                    }
                }
            }
        }

        public class Cursor : ICursor3D<T>
        {
            private LimitedContainer3D<T> _container;
            public int Index;
            public Vector3i? _Position;
            public Vector3i Position => _Position ?? _container.Position(Index);

            public T Value
            {
                get => _container[Index];
                set => _container[Index] = value;
            }

            public Cursor(LimitedContainer3D<T> container)
            {
                _container = container;
            }
            
            public T Relative(Vector3i direction, in T _default)
            {
                Vector3i pos = Position + direction;
                return _container.InBounds(pos) ? _container[pos] : _default;
            }
        }
    }
}