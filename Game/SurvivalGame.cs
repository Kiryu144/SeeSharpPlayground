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
        Mesh _mesh;
        
        public SurvivalGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.DebugMessageCallback((source, type, id, severity, length, message, param) =>
            {
                string messageString = Marshal.PtrToStringAnsi(message, length);
                System.Console.WriteLine($"OpenGL Error: id = {id}, severity = {severity}, message = {messageString}");
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
            _mesh = quad.Build();

            GL.Disable(EnableCap.CullFace);
            
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            //_renderer.ViewMatrix = Matrix4.Identity;
            _renderer.ViewMatrix = Matrix4.LookAt(5.0f, 5.0f, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            _renderer.Render(_mesh, PrimitiveType.Triangles, Shaders.PositionShader);
            
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
            _renderer.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80.0f),  (float) e.Width / e.Height, 0.1f, 100.0f);
        }
    }
}