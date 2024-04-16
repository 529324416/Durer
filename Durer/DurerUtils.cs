using SkiaSharp;

namespace Durer
{
    public static class DurerUtils
    {
        public static SKPoint[] GenRandomPoints(SKPoint lb, SKPoint rt, int count, int seed = 0)
        {
            float width = rt.X - lb.X;
            float height = rt.Y - lb.Y;
            var points = new SKPoint[count];
            var random = new System.Random(seed);
            for (int i = 0; i < count; i++)
            {
                points[i] = new SKPoint(
                    random.NextSingle() * width + lb.X,
                    random.NextSingle() * height + lb.Y
                );
            }
            return points;
        }

        public static SKPoint Lerp(SKPoint a, SKPoint b, float t) =>
            new(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);

        /// <summary>获取贝塞尔曲线上的点</summary>
        public static SKPoint CubicBezierCurve(SKPoint start, SKPoint c1, SKPoint c2, SKPoint end, float t)
        {
            SKPoint p0 = Lerp(start, c1, t);
            SKPoint p1 = Lerp(c1, c2, t);
            SKPoint p2 = Lerp(c2, end, t);

            SKPoint q0 = Lerp(p0, p1, t);
            SKPoint q1 = Lerp(p1, p2, t);

            return Lerp(q0, q1, t);
        }

        /// <summary>获取指定位置的贝塞尔曲线角度</summary>
        public static float GetAngleOfBezierCurve(
            SKPoint start,
            SKPoint c1,
            SKPoint c2,
            SKPoint end,
            float t1 = 0.99f
        ){
            float t2 = t1 + 0.001f;
            t1 -= 0.001f;

            var curvePoint1 = CubicBezierCurve(start, c1, c2, end, t1);
            var curvePoint2 = CubicBezierCurve(start, c1, c2, end, t2);
            return (float)Math.Atan2(curvePoint2.Y - curvePoint1.Y, curvePoint2.X - curvePoint1.X);
        }

        public static SKPoint GetTextAnchor(this DurerDirection direction)
        {
            return direction switch
            {
                DurerDirection.Top => new SKPoint(0.5f, 1),
                DurerDirection.Bottom => new SKPoint(0.5f, 0),
                DurerDirection.Left => new SKPoint(0, 0.5f),
                DurerDirection.Right => new SKPoint(1, 0.5f),
                DurerDirection.TopLeft => new SKPoint(0, 1),
                DurerDirection.TopRight => new SKPoint(1, 1),
                DurerDirection.BottomLeft => new SKPoint(0, 0),
                DurerDirection.BottomRight => new SKPoint(1, 0),
                _ => new SKPoint(0.5f, 0.5f)
            };
        }

        public static SKPoint GetTextPadding(
            this DurerDirection direction, 
            float textW,
            float textH,
            float padding
        ){
            return direction switch
            {
                DurerDirection.Top => new SKPoint(0, textH * 0.5f + padding),
                DurerDirection.Bottom => new SKPoint(0, -textH * 0.5f - padding),
                DurerDirection.Left => new SKPoint(-textW * 0.5f - padding, 0),
                DurerDirection.Right => new SKPoint(textW * 0.5f + padding, 0),
                DurerDirection.TopLeft => new SKPoint(-textW * 0.5f - padding, textH * 0.5f + padding),
                DurerDirection.TopRight => new SKPoint(textW * 0.5f + padding, textH * 0.5f + padding),
                DurerDirection.BottomLeft => new SKPoint(-textW * 0.5f - padding, -textH * 0.5f - padding),
                DurerDirection.BottomRight => new SKPoint(textW * 0.5f + padding, -textH * 0.5f - padding),
                _ => new SKPoint(0, 0)
            };
        }

        public static bool SavePng(this SKImage image, string filepath, int quality = 100) => SaveFile(image, filepath, SKEncodedImageFormat.Png, quality);
        public static bool SaveFile(this SKImage image, string filepath, SKEncodedImageFormat encodeType, int quality = 100)
        {
            try
            {
                var stream = File.OpenWrite(filepath);
                image.Encode(encodeType, quality).SaveTo(stream);
                stream.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }

    public static class ColorUtils{

        /// <summary>如果给定的色彩为空，则返回默认色彩</summary>
        public static SKColor GetColorOrDefault(this SKColor color, SKColor defaultColor){
            return color == default ? defaultColor : color;
        }

        /// <summary>判断颜色是否比另一个颜色更亮</summary>
        public static bool IsLighterThan(this SKColor color, SKColor anotherColor){
            return color.Red + color.Green + color.Blue > 
                anotherColor.Red + anotherColor.Green + anotherColor.Blue;
        }
    }

    public static class FontUtils{
        
        public static SKFontStyle GetFontStyleOrDefault(this SKFontStyle fontStyle, SKFontStyle defaultFontStyle){
            return fontStyle == default ? defaultFontStyle : fontStyle;
        }

        public static SKPoint GetOffsetOfChart(DurerDirection direction, float w, float h, float padding = 5f){

            return direction switch{
                DurerDirection.Top => new SKPoint(0, h + padding),
                DurerDirection.Bottom => new SKPoint(0, -h - padding),
                DurerDirection.Left => new SKPoint(w + padding, 0),
                DurerDirection.Right => new SKPoint(-w - padding, 0),
                DurerDirection.TopLeft => new SKPoint(w + padding, h + padding),
                DurerDirection.TopRight => new SKPoint(-w - padding, h + padding),
                DurerDirection.BottomLeft => new SKPoint(w + padding, -h - padding),
                DurerDirection.BottomRight => new SKPoint(-w - padding, -h - padding),
                _ => new SKPoint(0, 0)
            };
        }

        public static SKPoint GetOffsetOfDevice(DurerDirection direction, float w, float h, float padding = 5f){

            return direction switch{
                DurerDirection.Top => new SKPoint(0, -h - padding),
                DurerDirection.Bottom => new SKPoint(0, h + padding),
                DurerDirection.Left => new SKPoint(-w - padding, 0),
                DurerDirection.Right => new SKPoint(w + padding, 0),
                DurerDirection.TopLeft => new SKPoint(-w - padding, -h - padding),
                DurerDirection.TopRight => new SKPoint(w + padding, -h - padding),
                DurerDirection.BottomLeft => new SKPoint(-w - padding, h + padding),
                DurerDirection.BottomRight => new SKPoint(w + padding, h + padding),
                _ => new SKPoint(0, 0)
            };
        }
    }

    public class DurerShaderUtils
    {    
        /// <summary>创建一个渐变的Shader,渐变色由strokeColor过渡到backgroundColor</summary>
        public static SKShader FadeShader(
            float canvasWidth, 
            float canvasHeight,
            SKColor backgroundColor,
            SKColor strokeColor,
            float fadeWidth = 0.15f
        ){
            SKColor[] colors = new SKColor[]{
                backgroundColor,
                strokeColor,
                strokeColor,
                backgroundColor,
            };
            float[] pos = new float[]{0, fadeWidth, 1 - fadeWidth, 1};

            SKShader shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(0, canvasHeight),
                colors,
                pos,
                SKShaderTileMode.Repeat
            );
            SKShader shader2 = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(canvasWidth, 0),
                colors,
                pos,
                SKShaderTileMode.Clamp
            );
            var blendMode = strokeColor.IsLighterThan(backgroundColor) ? SKBlendMode.Darken : SKBlendMode.Lighten;
            return SKShader.CreateCompose(shader, shader2, blendMode);
        }
    }


}