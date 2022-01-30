﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Game.Game.Container;
using Game.Game.World;
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

        Mesh _voxels;
        
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
            
            LimitedContainer3D<Voxel> voxels = new LimitedContainer3D<Voxel>(16);
            foreach (var cursor in voxels.GetRegion(new Vector3i(0, 0, 0), new Vector3i(15, 10, 15)))
            {
                // Stone
                cursor.Value = new Voxel(0xFF5C5C5C);
            }
            foreach (var cursor in voxels.GetRegion(new Vector3i(0, 10, 0), new Vector3i(15, 1, 15)))
            {
                // Grass
                cursor.Value = new Voxel(0xFF1B9400);
            }
            _voxels = VoxelMesher.CreateMesh(voxels);
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
            _renderer.ViewMatrix = Matrix4.LookAt(25.0f, 25.0f, 25.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            
            _renderer.MatrixStack.Push();
            _renderer.Render(_voxels, PrimitiveType.Triangles, Shaders.PositionColorShader);
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