using System;
using System.Runtime.InteropServices;
using Game.Game.World;
using Game.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Game
{
    public class SurvivalGame : GameWindow
    {
        public static Renderer Renderer = new();
        private World _world;

        public SurvivalGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.DebugMessageCallback((source, type, id, severity, length, message, param) =>
            {
                if (severity != DebugSeverity.DebugSeverityNotification)
                {
                    string messageString = Marshal.PtrToStringAnsi(message, length);
                    Console.WriteLine($"OpenGL Error: id = {id}, severity = {severity}, message = {messageString}");
                }
            }, 0);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            
            _world = new World();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            Context.SwapBuffers();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Renderer.ViewMatrix = Matrix4.Identity;
            
            Renderer.MatrixStack.Push();
            _world.Tick();
            Renderer.MatrixStack.Pop();
        }

        public override void ProcessEvents()
        {
            base.ProcessEvents();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            Renderer.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80.0f),  (float) e.Width / e.Height, 0.1f, 100.0f);
        }
    }
}