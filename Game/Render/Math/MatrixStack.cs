using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Game.Render.Math
{
    public class MatrixStack
    {
        Stack<Matrix4> _stack = new();

        public void Push()
        {
            _stack.Push(Matrix4.Identity);
        }
        
        public void Pop()
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("Cannot pop stack more.");
            }
            _stack.Pop();
        }
        
        public void Translate(float x, float y, float z)
        {
            var m = _stack.Pop();
            _stack.Push(m * Matrix4.CreateTranslation(x, y, z));
        }
        
        public void Scale(float x, float y, float z)
        {
            var m = _stack.Pop();
            _stack.Push(m * Matrix4.CreateScale(x, y, z));
        }
        
        public void Rotate(float radians, float x, float y, float z)
        {
            var m = _stack.Pop();
            //_stack.Push(m * Matrix4.Rotate(new Vector3d(x, y, z), radians));
        }
        
        public Matrix4 Combine()
        {
            Matrix4 m = Matrix4.Identity;

            foreach (var matrix in _stack)
            {
                m *= matrix;
            }
            
            return m;
        }
    }
}