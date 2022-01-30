namespace Game.Game.World
{
    public struct Voxel
    {
        public static readonly Voxel Air = new();
        
        public uint Color;

        public Voxel(uint color = 0)
        {
            Color = color;
        }

        public bool IsInvisible()
        {
            return ((Color >> 24) & 0xFF) == 0;
        }
    }
}