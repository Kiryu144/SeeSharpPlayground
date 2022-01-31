using System;
using System.Threading;
using Game.Game.Container;
using Game.Render.Buffer;
using Game.Render.Shader;
using OpenTK.Graphics.OpenGL;

namespace Game.Game.World
{
    public class Chunk
    {
        /* The actual side length of a chunk. Always a power of 2. */
        public static readonly int SideLength = 32;
        
        public LimitedContainer3D<Voxel> Voxels = new(SideLength);
        public Mesh Mesh;
        private MeshBuilder _meshBuilder;
        private Thread _meshWorker;
        
        public void Tick()
        {
            if (_meshBuilder != null)
            {
                Mesh = _meshBuilder.Build();
                _meshBuilder = null;
            }
            
            if (Mesh != null)
            {
                SurvivalGame.Renderer.Render(Mesh, PrimitiveType.Triangles, Shaders.PositionColorShader);
            }
        }

        public void GenerateMesh()
        {
            _meshWorker = new Thread(() =>
            {
               _meshBuilder = VoxelMesher.CreateMesh(Voxels);
            });
            _meshWorker.Start();
        }
    }
}