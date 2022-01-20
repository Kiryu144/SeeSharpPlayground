using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Game.Render.Buffer
{
    public class MeshBuilder
    {
        private List<float> _positions = new();
        private List<float> _normals = new();
        private List<float> _uvs = new();
        private List<float> _colors = new(); 
        private List<uint> _indicies = new(); 
        
        private Vector3? _currentPosition;
        private Vector3? _currentNormal;
        private Vector2? _currentUv;
        private Vector4? _currentColor;

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
        
        public MeshBuilder Color(Vector4 color)
        {
            _currentColor = color;
            return this;
        }
        
        public unsafe void EndVertex()
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
            
            _positions.Add(_currentPosition.Value.X);
            _positions.Add(_currentPosition.Value.Y);
            _positions.Add(_currentPosition.Value.Z);
            
            if (_currentNormal != null)
            {
                //_normals.Add(_currentNormal.Value);
            }
            
            if (_currentUv != null)
            {
               // _uvs.Add(_currentUv.Value);
            }
            
            if (_currentColor != null)
            {
               // _colors.Add(_currentColor.Value);
            }
            
            _indicies.Add((uint) (_positions.Count - 1));
            
            _currentPosition = null;
            _currentNormal = null;
            _currentUv = null;
            _currentColor = null;
        }
        
        public unsafe Mesh Build()
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
            GL.BufferData(BufferTargetARB.ArrayBuffer, _positions.Count * sizeof(float), _positions[0], BufferUsageARB.StaticDraw);
            GL.EnableVertexAttribArray(vertexAttribPointerLocation);
            GL.VertexAttribPointer(vertexAttribPointerLocation++, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, BufferHandle.Zero);
            
            
            if (_normals.Count > 0)
            {
                var normalBuffer = GL.GenBuffer();
                buffers.Add(normalBuffer);
                GL.BindBuffer(BufferTargetARB.ArrayBuffer, normalBuffer);
                GL.BufferData(BufferTargetARB.ArrayBuffer, _normals.Count * sizeof(Vector3), _normals[0], BufferUsageARB.StaticDraw);
                GL.EnableVertexAttribArray(vertexAttribPointerLocation);
                GL.VertexAttribPointer(vertexAttribPointerLocation++, 3, VertexAttribPointerType.Float, false, 0, 0);
            }
            
            if (_uvs.Count > 0)
            {
                var uvBuffer = GL.GenBuffer();
                buffers.Add(uvBuffer);
                GL.BindBuffer(BufferTargetARB.ArrayBuffer, uvBuffer);
                GL.BufferData(BufferTargetARB.ArrayBuffer, _uvs.Count * sizeof(Vector2), _uvs[0], BufferUsageARB.StaticDraw);
                GL.EnableVertexAttribArray(vertexAttribPointerLocation);
                GL.VertexAttribPointer(vertexAttribPointerLocation++, 2, VertexAttribPointerType.Float, false, 0, 0);
            }
            
            if (_colors.Count > 0)
            {
                var colorBuffer = GL.GenBuffer();
                buffers.Add(colorBuffer);
                GL.BindBuffer(BufferTargetARB.ArrayBuffer, colorBuffer);
                GL.BufferData(BufferTargetARB.ArrayBuffer, _colors.Count * sizeof(Vector4), _colors[0], BufferUsageARB.StaticDraw);
                GL.EnableVertexAttribArray(vertexAttribPointerLocation);
                GL.VertexAttribPointer(vertexAttribPointerLocation, 4, VertexAttribPointerType.Float, false, 0, 0);
            }
            
            var indexBuffer = GL.GenBuffer();
            buffers.Add(indexBuffer);
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTargetARB.ElementArrayBuffer, _indicies.Count * sizeof(uint), _indicies[0], BufferUsageARB.StaticDraw);
            
            GL.BindVertexArray(VertexArrayHandle.Zero);

            return new Mesh(vao, _positions.Count / 3, buffers);
        }
    }
}