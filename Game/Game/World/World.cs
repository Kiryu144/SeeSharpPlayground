using System;
using DotnetNoise;
using Game.Game.Container;
using OpenTK.Mathematics;

namespace Game.Game.World
{
    public class World
    {
        public EndlessContainer3D<Chunk> Chunks = new();

        public World()
        {
            var noise = new FastNoise();
            
            int radius = 10;
            for (int x = -radius; x <= radius; ++x)
            {
                for (int z = -radius; z <= radius; ++z)
                {
                    Chunk chunk = new Chunk();
                    Chunks[new Vector3i(x, 0, z)] = chunk;
                    
                    foreach (var voxel in chunk.Voxels.GetRegion(new Vector3i(0, 0, 0), new Vector3i(Chunk.SideLength, 5, Chunk.SideLength)))
                    {
                        float v = noise.GetPerlin(x * Chunk.SideLength + voxel.Position.X, 0, z * Chunk.SideLength + voxel.Position.Z);
                        voxel.Value = new Voxel(new Color(128, (byte) (v * Byte.MaxValue), 128, 255));
                    }
                    
                    chunk.GenerateMesh();
                }
            }
        }

        public void Tick()
        {
            var matrixStack = SurvivalGame.Renderer.MatrixStack;
            foreach (var chunk in Chunks.GetAll())
            {
                matrixStack.Push();
                matrixStack.Translate(chunk.Position * new Vector3(Chunk.SideLength));
                chunk.Value.Tick();
                matrixStack.Pop();
            }
        }
    }
}