using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Color = Game.Game.Container.Color;

namespace Game.Render.Buffer
{
    public class MeshBuilder
    {
        private List<Vector3> _positions = new();
        private List<Vector3> _normals = new();
        private List<Vector2> _uvs = new();
        private List<uint> _colors = new(); 
        private List<uint> _indicies = new(); 
        
        private Vector3? _currentPosition;
        private Vector3? _currentNormal;
        private Vector2? _currentUv;
        private uint? _currentColor;

        public MeshBuilder Position(Vector3 position)
        {
            _currentPosition = position;
            return this;
        }
        
        public MeshBuilder Normal(Vector3 normal)
        {
            _currentNormal = normal;
            return this;
        }
        
        public MeshBuilder Uv(Vector2 uv)
        {
            _currentUv = uv;
            return this;
        }

        public MeshBuilder Color(Color color)
        {
            _currentColor = color.OpenGLColor;
            return this;
        }
        
        public void EndVertex()
        {
            if (_currentPosition == null)
            {
                throw new InvalidOperationException($"Cannot EndVertex() without at least Position() defined.");
            }
            
            if (_positions.Count > 0)
            {
                if (_normals.Count > 0 && _currentNormal == null)
                {
                    throw new InvalidOperationException($"Cannot EndVertex(). Normal() missing.");
                }
                if (_uvs.Count > 0 && _currentUv == null)
                {
                    throw new InvalidOperationException($"Cannot EndVertex(). UV() missing.");
                }
                if (_colors.Count > 0 && _currentColor == null)
                {
                    throw new InvalidOperationException($"Cannot EndVertex(). Color() missing.");
                }
            }
            
            _positions.Add(_currentPosition.Value); 
            
            if (_currentNormal != null)
            {
                _normals.Add(_currentNormal.Value);
            }
            
            if (_currentUv != null)
            {
                _uvs.Add(_currentUv.Value);
            }
            
            if (_currentColor != null)
            {
                _colors.Add(_currentColor.Value);
            }
            
            _indicies.Add((uint) _indicies.Count);
            _indicies.Add((uint) _indicies.Count);
            _indicies.Add((uint) _indicies.Count);
            
            _currentPosition = null;
            _currentNormal = null;
            _currentUv = null;
            _currentColor = null;
        }
        
        public Mesh Build()
        {
            if (_positions.Count == 0)
            {
                throw new InvalidOperationException("Cannot build without vertices.");
            }
            
            List<BufferHandle> buffers = new();

            uint vertexAttribPointerLocation = 0;
            var vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            
            var positionBuffer = GL.GenBuffer();
            buffers.Add(positionBuffer);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, positionBuffer);
            GL.BufferData(BufferTargetARB.ArrayBuffer, _positions.ToArray(), BufferUsageARB.StaticDraw);
            GL.EnableVertexAttribArray(vertexAttribPointerLocation);
            GL.VertexAttribPointer(vertexAttribPointerLocation++, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, BufferHandle.Zero);
            
            
            if (_normals.Count > 0)
            {
                var normalBuffer = GL.GenBuffer();
                buffers.Add(normalBuffer);
                GL.BindBuffer(BufferTargetARB.ArrayBuffer, normalBuffer);
                GL.BufferData(BufferTargetARB.ArrayBuffer, _normals.ToArray(), BufferUsageARB.StaticDraw);
                GL.EnableVertexAttribArray(vertexAttribPointerLocation);
                GL.VertexAttribPointer(vertexAttribPointerLocation++, 3, VertexAttribPointerType.Float, false, 0, 0);
            }
            
            if (_uvs.Count > 0)
            {
                var uvBuffer = GL.GenBuffer();
                buffers.Add(uvBuffer);
                GL.BindBuffer(BufferTargetARB.ArrayBuffer, uvBuffer);
                GL.BufferData(BufferTargetARB.ArrayBuffer, _uvs.ToArray(), BufferUsageARB.StaticDraw);
                GL.EnableVertexAttribArray(vertexAttribPointerLocation);
                GL.VertexAttribPointer(vertexAttribPointerLocation++, 2, VertexAttribPointerType.Float, false, 0, 0);
            }
            
            if (_colors.Count > 0)
            {
                var colorBuffer = GL.GenBuffer();
                buffers.Add(colorBuffer);
                GL.BindBuffer(BufferTargetARB.ArrayBuffer, colorBuffer);
                GL.BufferData(BufferTargetARB.ArrayBuffer, _colors.ToArray(), BufferUsageARB.StaticDraw);
                GL.EnableVertexAttribArray(vertexAttribPointerLocation);
                GL.VertexAttribPointer(vertexAttribPointerLocation, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);
            }
            
            var indexBuffer = GL.GenBuffer();
            buffers.Add(indexBuffer);
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTargetARB.ElementArrayBuffer, _indicies.ToArray(), BufferUsageARB.StaticDraw);
            
            GL.BindVertexArray(VertexArrayHandle.Zero);

            return new Mesh(vao, _positions.Count, buffers);
        }
    }
}