using System.Collections.Generic;
using OpenTK.Graphics;

namespace Game.Render.Buffer
{
    public class Mesh
    {
        public List<BufferHandle> _bufferHandles = new();
        public VertexArrayHandle VAO { get; } 
        public int VerticeCount { get; }

        public Mesh(VertexArrayHandle vao, int verticeCount, IEnumerable<BufferHandle> bufferHandles)
        {
            VAO = vao;
            VerticeCount = verticeCount;
            _bufferHandles.AddRange(bufferHandles);
        }
    }
}