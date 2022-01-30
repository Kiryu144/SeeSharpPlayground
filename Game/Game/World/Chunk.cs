using Game.Game.Container;
using OpenTK.Mathematics;

namespace Game.Game.World
{
    public class Chunk
    {
        /* The actual side length of a chunk. Always a power of 2. */
        public static readonly int SideLength = 32;
        
        private IContainer3D<Voxel> _voxels = new LimitedContainer3D<Voxel>(SideLength);

        public Chunk()
        {
            
        }
    }
}