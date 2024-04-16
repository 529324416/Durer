using SkiaSharp;
using Topten.RichTextKit;

namespace Durer
{
    public static partial class DurerDrawer
    {
        /// <summary>绘制一个简单的点组用于表示某个区域</summary>
        /// <param name="canvas">画布</param>
        /// <param name="matrix">坐标变换矩阵</param>
        /// <param name="x">区域左下角x坐标</param>
        /// <param name="y">区域左下角y坐标</param>
        /// <param name="w">区域宽度</param>
        /// <param name="h">区域高度</param>
        /// <param name="offsetX">x轴偏移</param>
        /// <param name="offsetY">y轴偏移</param>
        /// <param name="radius">点半径</param>
        /// <param name="color">点颜色</param>
        public static void DurerDrawField_Point(
            this SKCanvas canvas,
            SKMatrix matrix,
            int x,
            int y,
            int w,
            int h,
            float offsetX,
            float offsetY,
            float radius = 5f,
            SKColor color = default,
            SKShader? shader = null
        )
        {
            var paint = new SKPaint
            {
                IsAntialias = true,
                Color = color.GetColorOrDefault(SKColors.Black),
                Style = SKPaintStyle.Fill,
                Shader = shader,
            };

            /* 先偏移后转换 */
            matrix = SKMatrix.CreateTranslation(offsetX, offsetY).PostConcat(matrix);
            List<SKPoint> points = new();
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    points.Add(new SKPoint(i, j));
                }
            }
            foreach (var point in matrix.MapPoints(points.ToArray()))
            {
                canvas.DrawCircle(point, radius, paint);
            }
        }

        /// <summary>绘制一个简单的点组用于表示某个区域</summary>
        /// <param name="canvas">画布</param>
        /// <param name="matrix">坐标变换矩阵</param>
        /// <param name="x">区域左下角x坐标</param>
        /// <param name="y">区域左下角y坐标</param>
        /// <param name="w">区域宽度</param>
        /// <param name="h">区域高度</param>
        /// <param name="offsetX">x轴偏移</param>
        /// <param name="offsetY">y轴偏移</param>
        /// <param name="radius">点半径</param>
        /// <param name="color">点颜色</param>
        public static void DurerDrawField_Point(
            this SKCanvas canvas,
            SKMatrix matrix,
            DurerRectInt boundsInt,
            float offsetX,
            float offsetY,
            float radius = 5f,
            SKColor color = default,
            SKShader? shader = null
        )
        {
            DurerDrawField_Point(
                canvas,
                matrix,
                boundsInt.X,
                boundsInt.Y,
                boundsInt.Width,
                boundsInt.Height,
                offsetX,
                offsetY,
                radius,
                color,
                shader
            );
        }


        /// <summary>绘制一个简单的点组用于表示某个区域</summary>
        /// <param name="canvas">画布</param>
        /// <param name="matrix">坐标变换矩阵</param>
        public static void DurerDrawField_Box(
            this SKCanvas canvas,
            SKMatrix matrix,
            int x0,
            int y0,
            int x1,
            int y1,
            float offsetX,
            float offsetY,
            float width = 5f,
            float height = 5f,
            float lineWidth = 2.0f,
            SKColor color = default
        )
        {
            var paint = new SKPaint
            {
                IsAntialias = true,
                Color = color.GetColorOrDefault(SKColors.Black),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
            };

            /* 先偏移后转换 */
            matrix = SKMatrix.CreateTranslation(offsetX, offsetY)
            .PostConcat(matrix)
            .PostConcat(SKMatrix.CreateTranslation(-width * 0.5f, -height * 0.5f));
            var points = new List<SKPoint>();
            for (int i = x0; i < x1; i++)
            {
                for (int j = y0; j < y1; j++)
                {
                    points.Add(new SKPoint(i, j));
                }
            }
            foreach (var point in matrix.MapPoints(points.ToArray()))
            {
                canvas.DrawRect(point.X, point.Y, width, height, paint);
            }
        }



        public static void DurerDrawBorder(
            this SKCanvas canvas,
            SKMatrix matrix,
            float x0,
            float y0,
            float x1,
            float y1,
            float radiusx = 0f,
            float radiusy = 0f,
            float lineWidth = 2.0f,
            SKColor borderColor = default
        )
        {

            var paint = new SKPaint
            {
                IsAntialias = true,
                Color = borderColor,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
            };
            var pt0 = matrix.MapPoint(x0, y0);
            var pt1 = matrix.MapPoint(x1, y1);
            canvas.DrawRoundRect(pt0.X, pt0.Y, pt1.X - pt0.X, pt1.Y - pt0.Y, radiusx, radiusy, paint);
        }
    }


























    /// <summary>Durer绘图工具</summary>
    /// <remarks>数学绘图工具</remarks>
    public static class DurerDrawerMath
    {

        /// <summary>绘制线段</summary>
        public static void DurerDrawLineSegment(
            this SKCanvas canvas,
            float x1,
            float y1,
            float x2,
            float y2,
            SKColor color,
            float lineWidth,
            float endpointRadius,
            SKColor endpointColor,
            SKPathEffect? pathEffect = null,
            SKShader? shader = null
        ){
            var paint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                Shader = shader,
                StrokeCap = SKStrokeCap.Round,
            };
            if (pathEffect != null)
                paint.PathEffect = pathEffect;

            canvas.DrawLine(x1, y1, x2, y2, paint);
            var endpointPaint = new SKPaint
            {
                IsAntialias = true,
                Color = endpointColor
            };
            canvas.DrawCircle(x1, y1, endpointRadius, endpointPaint);
            canvas.DrawCircle(x2, y2, endpointRadius, endpointPaint);
        }

        /// <summary></summary>
        public static void DrawCircleMark(
            this SKCanvas canvas,
            float x, 
            float y,
            float ptRadius,
            SKColor ptColor,
            float outerRadius,
            SKColor outerColor,
            float outerLineWidth
        ){
            var paint = new SKPaint
            {
                IsAntialias = true,
                Color = ptColor,
                Style = SKPaintStyle.Fill,
            };
            canvas.DrawCircle(x, y, ptRadius, paint);
            paint.Color = outerColor;
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = outerLineWidth;
            canvas.DrawCircle(x, y, outerRadius, paint);
        }



        public static void DurerDrawMeasure(
            this SKCanvas canvas,
            SKMatrix coordinateMatrix,
            float x1,
            float y1,
            float x2,
            float y2,
            float measurePadding = 15.0f,
            float measureDst = 15.0f,
            int direction = 1,
            SKColor lineColor = default,
            float lineWidth = 1.8f,
            string label = "???",
            SKColor textColor = default,
            SKColor textBackgroundColor = default,
            string fontFamily = "Arial",
            int fontSize = 16
        )
        {
            lineColor = lineColor.GetColorOrDefault(SKColors.Black);

            var mappedPt0 = coordinateMatrix.MapPoint(x1, y1);
            var mappedPt1 = coordinateMatrix.MapPoint(x2, y2);

            var vector = new DurerVector2(mappedPt1.X - mappedPt0.X, mappedPt1.Y - mappedPt0.Y);
            var verticalVec = vector.Vertical(direction);
            var normalizeVertical = verticalVec.Normalize();

            var linePaint = new SKPaint
            {
                Color = lineColor,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                StrokeCap = SKStrokeCap.Round,
            };

            var offset = normalizeVertical * measurePadding;
            var distance = normalizeVertical * measureDst;

            var pt0_0 = new DurerVector2(mappedPt0) + offset;
            var pt0_1 = pt0_0 + distance;
            canvas.DrawLine(pt0_0.ToSKPoint(), pt0_1.ToSKPoint(), linePaint);

            var pt1_0 = new DurerVector2(mappedPt1) + offset;
            var pt1_1 = pt1_0 + distance;
            canvas.DrawLine(pt1_0.ToSKPoint(), pt1_1.ToSKPoint(), linePaint);

            var pt0_center = (pt0_0 + pt0_1) * 0.5f;
            var pt1_center = (pt1_0 + pt1_1) * 0.5f;
            // linePaint.PathEffect = SKPathEffect.CreateDash(new float[]{10, 10}, 0);
            canvas.DrawLine(pt0_center.ToSKPoint(), pt1_center.ToSKPoint(), linePaint);

            textColor = textColor.GetColorOrDefault(SKColors.Black);
            textBackgroundColor = textBackgroundColor.GetColorOrDefault(SKColors.White);
            var centerPoint = (pt0_center + pt1_center) * 0.5f;

            var textBlock = new RichString()
            .TextColor(textColor)
            .BackgroundColor(textBackgroundColor)
            .FontFamily(fontFamily)
            .FontSize(fontSize)
            .Alignment(TextAlignment.Center)
            .Add(label);

            centerPoint.y -= textBlock.MeasuredHeight * 0.5f;
            centerPoint.x -= textBlock.MeasuredWidth * 0.5f;

            textBlock.Paint(canvas, centerPoint.Sk);

            // canvas.ResetMatrix();

            // float angle = (float)Math.Atan2(y2 - y1, x2 - x1);
            // SKMatrix matrix = SKMatrix.CreateRotation(angle, x1, y1);
            // float lineLength = (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

            // SKMatrix borderMatrix = SKMatrix.CreateRotation(angle + MathF.PI * 0.5f, x1, y1);
            // canvas.SetMatrix(coordinateMatrix.PostConcat(borderMatrix));
            // canvas.DrawLine(x1, y1, x2, y2, new SKPaint
            // {
            //     Color = lineColor,
            //     IsAntialias = true,
            //     Style = SKPaintStyle.Stroke,
            //     StrokeWidth = lineWidth,
            //     Shader = shader,
            // });

            // var linePaint = new SKPaint
            // {
            //     Color = lineColor,
            //     IsAntialias = true,
            //     Style = SKPaintStyle.Stroke,
            //     StrokeWidth = lineWidth,
            //     StrokeCap = SKStrokeCap.Round,
            //     Shader = shader,
            // };

        }
    }


}