using System;
using System.Runtime.InteropServices;

namespace FractalMandelbrot
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public static Color Red = new Color(1, 0, 0);
        public static Color Green = new Color(0, 1, 0);
        public static Color Blue = new Color(0, 0, 1);
        public static Color White = new Color(1, 1, 1);
        public static Color Black = new Color(0, 0, 0);
        public static Color Transparent = new Color(0, 0, 0, 0);

        private static Random rand;

        public Color(float r, float g, float b, float a = 1f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static Color RandomColor()
        {
            if (rand == null)
                rand = new Random();
            return new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
        }

        public static Color operator +(Color first, Color second)
        {
            return new Color(first.r + second.r, first.g + second.g, first.b + second.b, first.a + second.a);
        }

        public static Color operator -(Color first, Color second)
        {
            return new Color(first.r - second.r, first.g - second.g, first.b - second.b, first.a - second.a);
        }

        public static Color operator *(Color color, float scalar)
        {
            return new Color(color.r * scalar, color.g * scalar, color.b * scalar, color.a * scalar);
        }

        public static Color operator /(Color color, float scalar)
        {
            return new Color(color.r / scalar, color.g / scalar, color.b / scalar, color.a / scalar);
        }

        public override string ToString()
        {
            return "r: " + r + "; g: " + g + "; b: " + b + "; a: " + a;
        }
    }
}