using SkiaSharp;

namespace Durer
{
    public class DurerArrow : DurerShape
    {
        public float scale;
        public float headAngleRadian;

        SKPoint topPointVec;
        SKPoint leftPointVec;
        SKPoint rightPointVec;

        public DurerArrow(float scale, float headAngleRadian = MathF.PI * 0.22222f)
        {
            this.scale = scale;
            this.headAngleRadian = MathF.Max(MathF.Min(headAngleRadian, MathF.PI * 0.25f), 0);

            topPointVec = new SKPoint(0, scale);
            leftPointVec = new SKPoint(-scale * MathF.Sin(headAngleRadian), -scale * MathF.Cos(headAngleRadian));
            rightPointVec = new SKPoint(scale * MathF.Sin(headAngleRadian), -scale * MathF.Cos(headAngleRadian));
        }
        public override void RotateRad(float radian)
        {
            float sin = MathF.Sin(radian);
            float cos = MathF.Cos(radian);
            topPointVec = new SKPoint(topPointVec.X * cos - topPointVec.Y * sin, topPointVec.X * sin + topPointVec.Y * cos);
            leftPointVec = new SKPoint(leftPointVec.X * cos - leftPointVec.Y * sin, leftPointVec.X * sin + leftPointVec.Y * cos);
            rightPointVec = new SKPoint(rightPointVec.X * cos - rightPointVec.Y * sin, rightPointVec.X * sin + rightPointVec.Y * cos);
        }
        public override SKPath GenPath(SKPoint mathPt)
        {
            var path = new SKPath();
            path.MoveTo(mathPt + topPointVec);
            path.LineTo(mathPt + leftPointVec);
            path.LineTo(mathPt + rightPointVec);
            return path;
        }
    }
}