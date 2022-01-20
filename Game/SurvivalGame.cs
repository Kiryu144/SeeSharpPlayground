using System;
using System.Runtime.InteropServices;
using Game.Render;
using Game.Render.Buffer;
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
        Renderer _renderer = new();
        
        Mesh _quad;
        Mesh _triangle;
        
        public SurvivalGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.DebugMessageCallback((source, type, id, severity, length, message, param) =>
            {
                string messageString = Marshal.PtrToStringAnsi(message, length);
                Console.WriteLine($"OpenGL Error: id = {id}, severity = {severity}, message = {messageString}");
            }, 0);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

            MeshBuilder quad = new MeshBuilder();
            quad.Position(new Vector3(0.0f, 0.0f, 0.0f)).EndVertex();
            quad.Position(new Vector3(0.0f, 1.0f, 0.0f)).EndVertex();
            quad.Position(new Vector3(1.0f, 1.0f, 0.0f)).EndVertex();
            quad.Position(new Vector3(1.0f, 0.0f, 0.0f)).EndVertex();
            quad.Position(new Vector3(0.0f, 0.0f, 0.0f)).EndVertex();
            quad.Position(new Vector3(1.0f, 1.0f, 0.0f)).EndVertex();
            _quad = quad.Build();
            
            MeshBuilder triangle = new MeshBuilder();
            triangle.Position(new Vector3(0.5f, 0.0f, 0.0f)).EndVertex();
            triangle.Position(new Vector3(1.0f, 1.0f, 0.0f)).EndVertex();
            triangle.Position(new Vector3(0.0f, 1.0f, 0.0f)).EndVertex();
            _triangle = triangle.Build();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            Context.SwapBuffers();
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _renderer.ViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            
            _renderer.MatrixStack.Push();
            
            _renderer.Render(_quad, PrimitiveType.Triangles, Shaders.PositionShader);
            _renderer.MatrixStack.Translate(0.0f, 1.0f, 0.0f);
            _renderer.Render(_triangle, PrimitiveType.Triangles, Shaders.PositionShader);
            
            _renderer.MatrixStack.Pop();
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _renderer.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80.0f),  (float) e.Width / e.Height, 0.1f, 100.0f);
        }
    }
}