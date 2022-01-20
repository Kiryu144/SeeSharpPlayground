using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindowSettings gameWindowSettings = new GameWindowSettings
            {
                RenderFrequency = 0.0,
                UpdateFrequency = 0.0,
                IsMultiThreaded = false
            };
            
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(1440, 900),
                Title = "SurvivalGame",
                StartFocused = true,
                APIVersion = new Version(4, 1)
            };
            
            using(SurvivalGame game = new SurvivalGame(gameWindowSettings, nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}