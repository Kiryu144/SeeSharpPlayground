using OpenTK.Graphics.OpenGL;

namespace Engine.Render.Shader
{
    public class PositionColorShader : BasicShader
    {
        private static readonly string vertex = 
            @"
                #version 410

                in vec3 position;
                in vec4 color;

                out vec4 _color;

                uniform mat4 modelViewMatrix;
                uniform mat4 projectionMatrix;

                void main() {
                    _color = color;
                    gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
                }
            ";
        
        private static readonly string fragment = 
            @"
                #version 410

                in vec4 _color;

                out vec4 color;

                void main() {
                    color = _color;
                }
            ";

        public PositionColorShader() : base(new Builder().WithSource(ShaderType.VertexShader, vertex).WithSource(ShaderType.FragmentShader, fragment).Build())
        { }
    }
}