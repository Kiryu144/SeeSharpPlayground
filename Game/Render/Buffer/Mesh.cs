using System.Collections.Generic;
using OpenTK.Graphics;

namespace Game.Render.Buffer
{
    public class Mesh
    {
        private List<BufferHandle> _bufferHandles = new();
        public VertexArrayHandle Vao { get; } 
        public int VerticeCount { get; }

        public Mesh(VertexArrayHandle vao, int verticeCount, IEnumerable<BufferHandle> bufferHandles)
        {
            Vao = vao;
            VerticeCount = verticeCount;
            _bufferHandles.AddRange(bufferHandles);
        }
    }
}