using OpenTK.Graphics.OpenGL;

namespace Game.Render.Shader
{
    public class PositionShader : BasicShader
    {
        private static readonly string vertex = 
            @"
                #version 150

                in vec3 position;

                uniform mat4 modelViewMatrix;
                uniform mat4 projectionMatrix;

                void main() {
                    gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
                }
            ";
        
        private static readonly string fragment = 
            @"
                #version 150

                out vec4 color;

                void main() {
                    color = vec4(1.0, 0.0, 1.0, 1.0);
                }
            ";

        public PositionShader() : base(new Builder().WithSource(ShaderType.VertexShader, vertex).WithSource(ShaderType.FragmentShader, fragment).Build())
        { }
    }
}