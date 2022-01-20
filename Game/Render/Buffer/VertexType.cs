using System;
using OpenTK.Graphics.OpenGL;

namespace Game.Render.Buffer
{
    public class VertexType
    {
        public VertexAttribPointerType Type { get; }
        public int Scalars { get; }

        public VertexType(VertexAttribPointerType type, int scalars)
        {
            Type = type;
            Scalars = scalars;
        }
        
        public static readonly VertexType Position = new(VertexAttribPointerType.Float, 3);
        public static readonly VertexType Normal = new(VertexAttribPointerType.Float, 3);
        public static readonly VertexType Uv = new(VertexAttribPointerType.Float, 2);
        public static readonly VertexType Color = new(VertexAttribPointerType.Float, 4);
    }
}