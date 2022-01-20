using Game.Render.Buffer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Game.Render.Shader
{
    public class BasicShader : ShaderProgram
    {
        int _uniformModelViewMatrix;
        int _uniformProjectionMatrix;
        
        public BasicShader(ProgramHandle handle) : base(handle)
        {
            _uniformModelViewMatrix = GetRequiredUniformLocation("modelViewMatrix");
            _uniformProjectionMatrix = GetRequiredUniformLocation("projectionMatrix");
            
            VertexTypes.Add(VertexType.Position);
        }

        public unsafe void SetModelViewMatrix(Matrix4 modelViewMatrix)
        {
            GL.UniformMatrix4fv(_uniformModelViewMatrix, 1, 0, &modelViewMatrix.Row0.X);
        }
        
        public unsafe void SetProjectionMatrix(Matrix4 projectionMatrix)
        {
            GL.UniformMatrix4fv(_uniformProjectionMatrix, 1, 0, &projectionMatrix.Row0.X);
        }
    }
}