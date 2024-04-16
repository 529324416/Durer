using SkiaSharp;

namespace Durer
{
    /// <summary>Durer基础绘图工具</summary>
    public static partial class DurerHelper
    {
        public static DurerCanvas CreateMathCanvas(
            SKSizeI size,
            SKPointI origin,
            int scale,
            int interval,
            int labelInterval,
            SKPointI padding,
            DurerStyle style
        ) =>
            CreateMathCanvas(
                size.Width,
                size.Height,
                origin.X,
                origin.Y,
                scale,
                interval,
                labelInterval,
                padding.X,
                padding.Y,
                style
            );

        public static DurerCanvas CreateMathCanvas(
            int width, int height,
            int originX, int originY,
            int scale,
            int interval,
            int labelInterval,
            int paddingX,
            int paddingY,
            DurerStyle style
        )
        {
            var canvas = new DurerCanvas(width, height);
            canvas.DrawBackground(style.backgroundColor);

            var padding = new SKPoint(paddingX, paddingY);
            var canvasSize = new SKSize(width - padding.X * 2, height - padding.Y * 2);
            canvas = canvas.SubCanvas(
                padding,
                canvasSize,
                padding,
                new SKPoint(1, 1)
            );

            canvas = canvas.SubCanvas(
                new SKPoint(0, 0),
                canvasSize,
                new SKPoint(originX, originY),
                new SKPoint(scale, scale)
            );
            DrawMathCoordinates(canvas, style, interval, labelInterval, true);
            return canvas;
        }

        public static void DrawMathCoordinates(
            DurerCanvas canvas,
            DurerStyle style,
            float interval = 5,
            float labelInterval = 5,
            bool fade = true
        )
        {
            canvas.DrawPanelOfCanvas(style);
            var shader = fade ? style.CreateFadeShader(canvas.coord.Width, canvas.coord.Height, style.mathStyle.gridColor) : null;
            canvas.DrawGridsOfCanvas(1, 1, style.mathStyle.gridWidth, style.mathStyle.gridColor, shader);

            shader = fade ? style.CreateFadeShader(canvas.coord.Width, canvas.coord.Height, style.mathStyle.gridColorLarge) : null;
            canvas.DrawGridsOfCanvas(interval, interval, style.mathStyle.gridWidthLarge, style.mathStyle.gridColorLarge, shader);

            shader = fade ? style.CreateFadeShader(canvas.coord.Width, canvas.coord.Height, style.mathStyle.axisColor) : null;
            canvas.DrawAxisRaw(style.mathStyle.axisColor, style.mathStyle.axisWidth, shader);

            canvas.DrawAxisLabels(
                labelInterval,
                labelInterval,
                style.mathStyle.axisColor,
                fontFamily: style.fontStyle.fontFamily,
                fontSize: style.fontStyle.fontSize,
                offset: new SKPoint(5, -5),
                anchor: new SKPoint(0, 1f)
            );
        }


    }
}