using System;
using System.Runtime.InteropServices;
using Game.Render.Shader;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Game
{
    public class SurvivalGame : GameWindow
    {
        private Matrix4 _projectionMatrix;
        private Matrix4 _modelViewMatrix;
        
        private VertexArrayHandle _vao;
        private BufferHandle _vertexBuffer;
        
        public SurvivalGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.DebugMessageCallback((source, type, id, severity, length, message, param) =>
            {
                string messageString = Marshal.PtrToStringAnsi(message, length);
                System.Console.WriteLine($"OpenGL Error: id = {id}, severity = {severity}, message = {messageString}");
            }, 0);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

            float[] vertices = {
                0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                1.0f, 0.0f, 0.0f
            };
            
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTargetARB.ArrayBuffer, vertices, BufferUsageARB.StaticDraw);
            
            GL.Disable(EnableCap.CullFace);
            
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            _modelViewMatrix = Matrix4.Identity;
            
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _modelViewMatrix *= Matrix4.CreateRotationY((float) Math.Sin(milliseconds / 1000D) * 2.0f);
            
            _modelViewMatrix *= Matrix4.LookAt(5.0f, 5.0f, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f); 
            
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            Shaders.PositionShader.Use();
            Shaders.PositionShader.SetProjectionMatrix(_projectionMatrix);
            Shaders.PositionShader.SetModelViewMatrix(_modelViewMatrix);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);


            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80.0f),  (float) e.Width / e.Height, 0.1f, 100.0f, out _projectionMatrix);
        }
    }
}