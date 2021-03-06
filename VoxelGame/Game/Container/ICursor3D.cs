using OpenTK.Mathematics;

namespace Game.Game.Container
{
    public interface ICursor3D<T>
    {
        T Value { get; set; }   
        Vector3i Position { get; }
        
        T Relative(Vector3i direction, in T _default);
    }
}