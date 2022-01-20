using System;
using System.Collections.Generic;
using System.IO;
using Game.Render.Buffer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Render.Shader
{
    public abstract class ShaderProgram
    {
        ProgramHandle _programHandle;
        public List<VertexType> VertexTypes { get; } = new();

        public ShaderProgram(ProgramHandle handle)
        {
            _programHandle = handle;
        }

        public void Use()
        {
            GL.UseProgram(_programHandle);
        }

        protected int GetRequiredUniformLocation(string name)
        {
            int location = GL.GetUniformLocation(_programHandle, name);
            if (location < 0)
            {
                throw new ArgumentException($"Shader program {_programHandle} doesn't hold a uniform named '{name}'");
            }
            return location;
        }

        public class Builder
        {
            List<ShaderHandle> _shaderHandles = new();

            public Builder WithSource(ShaderType shaderType, string source)
            {
                var shader = GL.CreateShader(shaderType);
                GL.ShaderSource(shader, source);

                GL.GetShaderInfoLog(shader, out var error);
                if (error != string.Empty)
                {
                    GL.DeleteShader(shader);
                    throw new InvalidOperationException($"Unable to compile {shaderType}: {error}");
                }
                
                _shaderHandles.Add(shader);
                return this;
            }
            
            public Builder WithFile(ShaderType shaderType, string filepath)
            {
                return WithSource(shaderType, File.ReadAllText(filepath));
            }
            
            public ProgramHandle Build()
            {
                var program = GL.CreateProgram();
                foreach (var shaderHandle in _shaderHandles)
                {
                    GL.AttachShader(program, shaderHandle);
                }
                GL.LinkProgram(program);
                
                GL.GetProgramInfoLog(program, out var error);
                if (error != string.Empty)
                {
                    GL.DeleteProgram(program);
                    throw new InvalidOperationException($"Unable to link program: {error}");
                }
                
                return program;
            }
        }
    }
}
































