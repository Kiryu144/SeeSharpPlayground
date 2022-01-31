using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Game.Game.Container
{
    public interface IContainer3D<T>
    {
        T this[Vector3i position] { get; set; }
        
        bool InBounds(Vector3i position);
        
        IEnumerable<ICursor3D<T>> GetAll();
        IEnumerable<ICursor3D<T>> GetRegion(Vector3i start, Vector3i size);
    }
}