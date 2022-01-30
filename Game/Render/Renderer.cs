using Game.Render.Buffer;
using Game.Render.Math;
using Game.Render.Shader;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Game.Render
{
    public class Renderer
    {
        public MatrixStack MatrixStack { get; } = new();
        public Matrix4 ViewMatrix = Matrix4.Identity;
        public Matrix4 ProjectionMatrix = Matrix4.Identity;
        
        public void Render(Mesh mesh, PrimitiveType type, ShaderProgram shader)
        {
            shader.Use();
            if (shader is BasicShader positionShader)
            {
                positionShader.SetProjectionMatrix(ProjectionMatrix);
                positionShader.SetModelViewMatrix(MatrixStack.Combine() * ViewMatrix);
            }

            GL.BindVertexArray(mesh.Vao);
            GL.DrawElements(type, mesh.VerticeCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(VertexArrayHandle.Zero);
        }
    }
}