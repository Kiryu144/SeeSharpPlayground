using System.Drawing;
using Game.Game.Container;
using Game.Render.Buffer;
using OpenTK;
using OpenTK.Mathematics;
using Color = Game.Game.Container.Color;

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
        
        private static readonly Vector3i[] AmbientOcclusion = {
			new ( 1, 0, -1 ),  new ( 1, -1, -1 ),  new ( 0, -1, -1 ), new ( 0, -1, -1 ), new ( -1, -1, -1 ), new ( -1, 0, -1 ),
			new ( 1, 0, -1 ),  new ( 1, 1, -1 ),   new ( 0, 1, -1 ),  new ( 0, 1, -1 ),	 new ( -1, 1, -1 ),	 new ( -1, 0, -1 ),
			new ( 1, 0, -1 ),  new ( 1, 1, -1 ),   new ( 0, 1, -1 ),  new ( 0, -1, -1 ), new ( -1, -1, -1 ), new ( -1, 0, -1 ),

			new ( -1, 0, -1 ), new ( -1, -1, -1 ), new ( -1, -1, 0 ), new ( -1, -1, 0 ), new ( -1, -1, 1 ),	 new ( -1, 0, 1 ),
			new ( -1, 0, -1 ), new ( -1, 1, -1 ),  new ( -1, 1, 0 ),  new ( -1, 1, 0 ),	 new ( -1, 1, 1 ),	 new ( -1, 0, 1 ),
			new ( -1, 0, -1 ), new ( -1, 1, -1 ),  new ( -1, 1, 0 ),  new ( -1, -1, 0 ), new ( -1, -1, 1 ),	 new ( -1, 0, 1 ),

			new ( -1, 0, 1 ),  new ( -1, -1, 1 ),  new ( 0, -1, 1 ),  new ( 0, -1, 1 ),	 new ( 1, -1, 1 ),	 new ( 1, 0, 1 ),
			new ( -1, 0, 1 ),  new ( -1, 1, 1 ),   new ( 0, 1, 1 ),	  new ( 0, 1, 1 ),	 new ( 1, 1, 1 ),	 new ( 1, 0, 1 ),
			new ( -1, 0, 1 ),  new ( -1, 1, 1 ),   new ( 0, 1, 1 ),	  new ( 0, -1, 1 ),	 new ( 1, -1, 1 ),	 new ( 1, 0, 1 ),

			new ( 1, 0, 1 ),   new ( 1, -1, 1 ),   new ( 1, -1, 0 ),  new ( 1, -1, 0 ),	 new ( 1, -1, -1 ),	 new ( 1, 0, -1 ),
			new ( 1, 0, 1 ),   new ( 1, 1, 1 ),	   new ( 1, 1, 0 ),	  new ( 1, 1, 0 ),	 new ( 1, 1, -1 ),	 new ( 1, 0, -1 ),
			new ( 1, 0, 1 ),   new ( 1, 1, 1 ),	   new ( 1, 1, 0 ),	  new ( 1, -1, 0 ),	 new ( 1, -1, -1 ),	 new ( 1, 0, -1 ),

			new ( 0, -1, -1 ), new ( -1, -1, -1 ), new ( -1, -1, 0 ), new ( 1, -1, 0 ),	 new ( 1, -1, -1 ),	 new ( 0, -1, -1 ),
			new ( 0, -1, 1 ),  new ( 1, -1, 1 ),   new ( 1, -1, 0 ),  new ( 0, -1, 1 ),	 new ( 1, -1, 1 ),	 new ( 1, -1, 0 ),
			new ( 0, -1, 1 ),  new ( -1, -1, 1 ),  new ( -1, -1, 0 ), new ( 0, -1, -1 ), new ( -1, -1, -1 ), new ( -1, -1, 0 ),

			new ( 0, 1, -1 ),  new ( -1, 1, -1 ),  new ( -1, 1, 0 ),  new ( -1, 1, 0 ),	 new ( -1, 1, 1 ),	 new ( 0, 1, 1 ),
			new ( 1, 1, 0 ),   new ( 1, 1, -1 ),   new ( 0, 1, -1 ),  new ( 1, 1, 0 ),	 new ( 1, 1, -1 ),	 new ( 0, 1, -1 ),
			new ( -1, 1, 0 ),  new ( -1, 1, 1 ),   new ( 0, 1, 1 ),	  new ( 1, 1, 0 ),	 new ( 1, 1, 1 ),	 new ( 0, 1, 1 ),
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
                    cursor.Neighbour(new Vector3i(0, -1, 0), in Voxel.Air).IsInvisible(),
                    cursor.Neighbour(new Vector3i(0, 1, 0), in Voxel.Air).IsInvisible()
                };
                
                for (var i = 0; i < touchingNeighbours.Length; i++)
                {
	                if (!touchingNeighbours[i])
	                {
		                continue;
	                }
	                
	                for (var j = 0; j < Faces[i].Length; j++)
	                {
		                int k = i * 18 + j * 3;
						int s1 = cursor.Neighbour(AmbientOcclusion[ k + 0 ], Voxel.Air).IsInvisible() ? 0 : 1;
						int s2 = cursor.Neighbour(AmbientOcclusion[ k + 2 ], Voxel.Air).IsInvisible() ? 0 : 1;
						int c = cursor.Neighbour(AmbientOcclusion[ k + 1 ], Voxel.Air).IsInvisible() ? 0 : 1;
						int acDarkness = ( s1 != 0 && s2 != 0 ) ? 3 : s1 + s2 + c;
						acDarkness *= 20;
						
						Color color = cursor.Value.Color;
						color.Darken(acDarkness);

						meshBuilder.Position(Faces[i][j] + cursor.Position).Color(color).EndVertex();
	                }
                }
            }
            
            return meshBuilder.Build();
        }
    }
}