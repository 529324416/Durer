using SkiaSharp;

namespace Durer
{
    /// <summary>正方形</summary>
    public class DurerSquare : DurerShape
    {
        public float scale;
        public SKPoint LT;
        public SKPoint LB;
        public SKPoint RT;
        public SKPoint RB;

        public DurerSquare(float scale)
        {
            this.scale = scale;
            float halfScale = scale * 0.5f;
            LT = new SKPoint(halfScale, halfScale);
            LB = new SKPoint(halfScale, -halfScale);
            RT = new SKPoint(-halfScale, halfScale);
            RB = new SKPoint(-halfScale, -halfScale);
        }
        public override void RotateRad(float radian)
        {
            float sin = MathF.Sin(radian);
            float cos = MathF.Cos(radian);
            LT = new SKPoint(LT.X * cos - LT.Y * sin, LT.X * sin + LT.Y * cos);
            LB = new SKPoint(LB.X * cos - LB.Y * sin, LB.X * sin + LB.Y * cos);
            RT = new SKPoint(RT.X * cos - RT.Y * sin, RT.X * sin + RT.Y * cos);
            RB = new SKPoint(RB.X * cos - RB.Y * sin, RB.X * sin + RB.Y * cos);
        }
        public override SKPath GenPath(SKPoint mathPt)
        {
            var path = new SKPath();
            path.MoveTo(mathPt + LT);
            path.LineTo(mathPt + LB);
            path.LineTo(mathPt + RB);
            path.LineTo(mathPt + RT);
            return path;
        }
    }
}