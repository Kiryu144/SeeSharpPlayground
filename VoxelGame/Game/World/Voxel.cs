using Engine.Render.Math;
using Game.Game.Container;

namespace Game.Game.World
{
    public struct Voxel
    {
        public static readonly Voxel Air = new();
        
        public Color Color;

        public Voxel(Color color)
        {
            Color = color;
        }

        public bool IsInvisible()
        {
            return Color.A == 0;
        }
    }
}