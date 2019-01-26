using System;
using System.Runtime.InteropServices;

namespace FractalMandelbrot
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public float x;
        public float y;

        public static Vector2 zero = new Vector2(0, 0);
        public static Vector2 one = new Vector2(1, 1);

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float GetLength()
        {
            return Distance(zero);
        }

        public float GetAngle(Vector2 point)
        {
            return (float)Math.Acos((x * point.x + y * point.y) / (GetLength() * point.GetLength()));
        }

        public float Distance(Vector2 point)
        {
            return (float)Math.Sqrt(Math.Abs((point.x - x) * (point.x - x) + ((point.y - y) * (point.y - y))));
        }

        public void SetLength(float length)
        {
            float k = y / x;
            float newx = (float)Math.Sqrt(Math.Pow(length, 2) / (1 + Math.Pow(k, 2)));
            float newy = (float)Math.Sqrt(Math.Pow(length, 2) - Math.Pow(newx, 2));
            if (x < 0)
                newx *= -1;
            if (y < 0)
                newy *= -1;
            x = newx;
            y = newy;
        }

        public float Slope(Vector2 point)
        {
            return (y - point.y) / (x - point.x);
        }

        public bool IsInBox(Vector2 start, Vector2 end)
        {
            return x <= end.x && y <= end.y && x >= start.x && y >= start.y;
        }

        public void Limit(Vector2 min, Vector2 max)
        {
            if (x < min.x)
                x = min.x;
            if (x > max.x)
                x = max.x;
            if (y < min.y)
                y = min.y;
            if (y > max.y)
                y = max.y;
        }

        public static Vector2 operator +(Vector2 first, Vector2 second)
        {
            return new Vector2(first.x + second.x, first.y + second.y);
        }

        public static Vector2 operator -(Vector2 first, Vector2 second)
        {
            return new Vector2(first.x - second.x, first.y - second.y);
        }

        public static Vector2 operator *(Vector2 first, Vector2 second)
        {
            return new Vector2(first.x * second.x, first.y * second.y);
        }

        public static Vector2 operator /(Vector2 first, Vector2 second)
        {
            return new Vector2(first.x / second.x, first.y / second.y);
        }

        public static bool operator >(Vector2 first, Vector2 second)
        {
            if (first.x > second.x && first.y > second.y)
                return true;
            return false;
        }

        public static bool operator >=(Vector2 first, Vector2 second)
        {
            if (first.x >= second.x && first.y >= second.y)
                return true;
            return false;
        }

        public static bool operator <(Vector2 first, Vector2 second)
        {
            if (first.x < second.x && first.y < second.y)
                return true;
            return false;
        }

        public static bool operator <=(Vector2 first, Vector2 second)
        {
            if (first.x <= second.x && first.y <= second.y)
                return true;
            return false;
        }

        public static bool operator ==(Vector2 first, Vector2 second)
        {
            return Math.Abs(first.x - second.x) < 1E-6 && Math.Abs(first.y - second.y) < 1E-6; //TODO EPSILON
        }

        public static bool operator !=(Vector2 first, Vector2 second)
        {
            return !(Math.Abs(first.x - second.x) < 1E-6 && Math.Abs(first.y - second.y) < 1E-6);
        }

        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2(-vector.x, -vector.y);
        }

        public override string ToString()
        {
            return x + ", " + y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector2 && Equals((Vector2)obj);
        }
    }
}
