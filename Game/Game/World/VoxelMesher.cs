using Game.Game.Container;
using Game.Render.Buffer;
using OpenTK.Mathematics;

namespace Game.Game.World
{
    public static class VoxelMesher
    {
        private static readonly Vector3[][] Faces =
        {
            new Vector3[]
            {
                // Front
                new ( 1.0f, 0.0f, 0.0f ), new ( 0.0f, 0.0f, 0.0f ), new ( 1.0f, 1.0f, 0.0f ),
		        new ( 0.0f, 1.0f, 0.0f ), new ( 1.0f, 1.0f, 0.0f ), new ( 0.0f, 0.0f, 0.0f )
            },
            new Vector3[]
            {
                // Right
                new ( 0.0f, 0.0f, 0.0f ), new ( 0.0f, 0.0f, 1.0f ), new ( 0.0f, 1.0f, 0.0f ),
		        new ( 0.0f, 1.0f, 1.0f ), new ( 0.0f, 1.0f, 0.0f ), new ( 0.0f, 0.0f, 1.0f )
            },
            new Vector3[]
            {
                // Back
                new ( 0.0f, 0.0f, 1.0f ), new ( 1.0f, 0.0f, 1.0f ), new ( 0.0f, 1.0f, 1.0f ),
		        new ( 1.0f, 1.0f, 1.0f ), new ( 0.0f, 1.0f, 1.0f ), new ( 1.0f, 0.0f, 1.0f )
            },
            new Vector3[]
            {
                // Left
                new ( 1.0f, 0.0f, 1.0f ), new ( 1.0f, 0.0f, 0.0f ), new ( 1.0f, 1.0f, 1.0f ),
		        new ( 1.0f, 1.0f, 0.0f ), new ( 1.0f, 1.0f, 1.0f ), new ( 1.0f, 0.0f, 0.0f )
            },
            new Vector3[]
            {
                // Top
                new ( 0.0f, 0.0f, 0.0f ), new ( 1.0f, 0.0f, 0.0f ), new ( 1.0f, 0.0f, 1.0f ),
		        new ( 1.0f, 0.0f, 1.0f ), new ( 0.0f, 0.0f, 1.0f ), new ( 0.0f, 0.0f, 0.0f )
            },
            new Vector3[]
            {
                // Bottom
                new ( 0.0f, 1.0f, 0.0f ), new ( 0.0f, 1.0f, 1.0f ), new ( 1.0f, 1.0f, 0.0f ),
		        new ( 1.0f, 1.0f, 0.0f ), new ( 0.0f, 1.0f, 1.0f ), new ( 1.0f, 1.0f, 1.0f )
            }
        };
        
        public static Mesh CreateMesh(IContainer3D<Voxel> voxels)
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            foreach (var cursor in voxels.GetAll())
            {
                if (cursor.Value.IsInvisible())
                {
                    continue;
                }
                
                bool[] touchingNeighbours =
                {
                    cursor.Neighbour(new Vector3i(0, 0, -1), in Voxel.Air).IsInvisible(),
                    cursor.Neighbour(new Vector3i(-1, 0, 0), in Voxel.Air).IsInvisible(),
                    cursor.Neighbour(new Vector3i(0, 0, 1), in Voxel.Air).IsInvisible(),
                    cursor.Neighbour(new Vector3i(1, 0, 0), in Voxel.Air).IsInvisible(),
                    cursor.Neighbour(new Vector3i(0, 1, 0), in Voxel.Air).IsInvisible(),
                    cursor.Neighbour(new Vector3i(0, 0, -1), in Voxel.Air).IsInvisible()
                };
                
                for (var i = 0; i < touchingNeighbours.Length; i++)
                {
                    foreach (var face in Faces[i])
                    {
                        meshBuilder.Position(face + cursor.Position).Color((uint) (cursor.Value.Color)).EndVertex();
                    }
                }
            }
            
            return meshBuilder.Build();
        }
    }
}