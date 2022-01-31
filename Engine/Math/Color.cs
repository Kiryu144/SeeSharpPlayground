using System;

namespace Engine.Render.Math
{
    public struct Color
    {
        public byte R, G, B, A;

        public uint OpenGLColor
        {
            get
            {
                uint value = R;
                value |= (uint) G << 8;
                value |= (uint) B << 16;
                value |= (uint) A << 24;
                return value;
            }
        }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public void Lighten(int amount)
        {
            R = (byte) System.Math.Clamp(amount + R, Byte.MinValue, Byte.MaxValue);
            G = (byte) System.Math.Clamp(amount + G, Byte.MinValue, Byte.MaxValue);
            B = (byte) System.Math.Clamp(amount + B, Byte.MinValue, Byte.MaxValue);
        }
        
        public void Darken(int amount)
        {
            Lighten(-amount);
        }
        
        public static Color FromARGB(uint argb)
        {
            return new Color((byte) ((argb >> 16) & 0xFF), (byte) ((argb >> 8) & 0xFF), (byte) ((argb >> 0) & 0xFF), (byte) ((argb >> 24) & 0xFF));
        }
    }
}