using SkiaSharp;

namespace Durer
{
    /// <summary>图勒坐标系系统</summary>
    public enum DurerDirection
    {

        Top,
        Bottom,
        Left,
        Right,

        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    /// <summary>2维向量</summary>
    public struct DurerVector2
    {
        public float x;
        public float y;

        public static readonly DurerVector2 Zero = new(0, 0);
        public static readonly DurerVector2 Up = new(0, 1);
        public static readonly DurerVector2 Right = new(1, 0);

        public readonly SKPoint Sk => new(x, y);
        public readonly SKPointI SkI => new((int)x, (int)y);

        public DurerVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public DurerVector2(SKPoint point)
        {
            x = point.X;
            y = point.Y;
        }

        public override readonly string ToString()
        {
            return $"({x}, {y})";
        }
        public readonly DurerVector2 Vertical(int direction = 1)
        {

            if (x == 0 && y == 0) return Zero;
            return new DurerVector2(-y, x) * Math.Sign(direction);
        }
        public readonly DurerVector2 Normalize()
        {

            if (x == 0) return new DurerVector2(0, Math.Sign(y));
            if (y == 0) return new DurerVector2(Math.Sign(x), 0);
            float dstReciprocal = 1 / MathF.Sqrt(x * x + y * y);
            return new DurerVector2(x * dstReciprocal, y * dstReciprocal);
        }
        public readonly float Modulus()
        {

            if (x == 0) return y;
            if (y == 0) return x;
            return MathF.Sqrt(x * x + y * y);
        }


        public readonly SKPoint ToSKPoint()
        {
            return new(x, y);
        }
        public readonly SKPointI ToSKPointI()
        {
            return new((int)x, (int)y);
        }
        public readonly SKSize ToSKSize()
        {
            return new(x, y);
        }
        public readonly SKSizeI ToSKSizeI()
        {
            return new((int)x, (int)y);
        }
        public static DurerVector2 operator +(DurerVector2 a, DurerVector2 b)
        {
            return new(a.x + b.x, a.y + b.y);
        }
        public static DurerVector2 operator -(DurerVector2 a, DurerVector2 b)
        {
            return new(a.x - b.x, a.y - b.y);
        }
        public static DurerVector2 operator *(DurerVector2 a, float b)
        {
            return new(a.x * b, a.y * b);
        }
        public static DurerVector2 operator *(float a, DurerVector2 b)
        {
            return new(a * b.x, a * b.y);
        }
        public static DurerVector2 operator /(DurerVector2 a, float b)
        {
            return new(a.x / b, a.y / b);
        }
        public static bool operator ==(DurerVector2 a, DurerVector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(DurerVector2 a, DurerVector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public override readonly bool Equals(object? obj)
        {

            if (obj is DurerVector2 vector2)
            {
                return this == vector2;
            }
            return false;
        }
        public override readonly int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }
    }

    public struct DurerVector2Int
    {
        public int X;
        public int Y;

        public DurerVector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override readonly string ToString()
        {
            return $"({X}, {Y})";
        }
        public readonly SKPoint ToSKPoint()
        {
            return new(X, Y);
        }
        public readonly SKPointI ToSKPointI()
        {
            return new(X, Y);
        }
        public readonly SKSize ToSKSize()
        {
            return new(X, Y);
        }
        public readonly SKSizeI ToSKSizeI()
        {
            return new(X, Y);
        }
        public static DurerVector2Int operator +(DurerVector2Int a, DurerVector2Int b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }
        public static DurerVector2Int operator +(DurerVector2Int a, int b)
        {
            return new(a.X + b, a.Y + b);
        }
        public static DurerVector2Int operator -(DurerVector2Int a, DurerVector2Int b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }
        public static DurerVector2Int operator -(DurerVector2Int a, int b)
        {
            return new(a.X - b, a.Y - b);
        }
        public static DurerVector2Int operator *(DurerVector2Int a, int b)
        {
            return new(a.X * b, a.Y * b);
        }
        public static DurerVector2Int operator /(DurerVector2Int a, int b)
        {
            return new(a.X / b, a.Y / b);
        }
    }

    /// <summary>
    /// Durer系统的边框描述信息
    /// </summary>
    /// <remarks>该对象的单位和坐标系一定是基于原始画布尺寸,但采用的是Chart坐标</remarks>
    public struct DurerRect
    {
        public float x;
        public float y;
        public float width;
        public float height;

        public readonly float Left => x;
        public readonly float Right => x + width;
        public readonly float Bottom => y;
        public readonly float Top => y + height;

        public DurerRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        public DurerRect(SKRect rect)
        {
            x = rect.Left;
            y = rect.Top;
            width = rect.Width;
            height = rect.Height;
        }
    }

    /// <summary>
    /// Durer系统的边框描述信息
    /// </summary>
    /// <remarks>该对象的单位和坐标系一定是基于原始画布尺寸,但采用的是Chart坐标</remarks>
    public struct DurerRectInt
    {
        public DurerVector2Int Position;
        public DurerVector2Int Size;
        public readonly int Left => Position.X;
        public readonly int Right => Position.X + Size.X;
        public readonly int Bottom => Position.Y;
        public readonly int Top => Position.Y + Size.Y;
        public readonly int X => Position.X;
        public readonly int Y => Position.Y;
        public readonly int Width => Size.X;
        public readonly int Height => Size.Y;

        public DurerRectInt(int x, int y, int width, int height)
        {
            Position = new DurerVector2Int(x, y);
            Size = new DurerVector2Int(width, height);
        }
        public DurerRectInt(SKRectI rect)
        {
            Position = new DurerVector2Int(rect.Left, rect.Top);
            Size = new DurerVector2Int(rect.Width, rect.Height);
        }
    }
}