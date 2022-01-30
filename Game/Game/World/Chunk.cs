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
        
        public void SetDefault()
        {
            foreach (var cursor in _voxels.GetRegion(new Vector3i(0, 0, 0), new Vector3i(SideLength, 10, SideLength)))
            {
                // Stone
                cursor.Value = new Voxel(0xFF5C5C5C);
            }
            
            foreach (var cursor in _voxels.GetRegion(new Vector3i(0, 10, 0), new Vector3i(SideLength, 11, SideLength)))
            {
                // Grass
                cursor.Value = new Voxel(0xFF1B9400);
            }
        }
    }
}