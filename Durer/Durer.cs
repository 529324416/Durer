using System.Drawing;
using SkiaSharp;
using Topten.RichTextKit;

namespace Durer
{

    public static class Colors
    {
        public readonly static SKColor Dark = new(22, 23, 28);
        public readonly static SKColor DarkDeep = new(31, 32, 45);
        public readonly static SKColor Light = new(255, 253, 227);
        public readonly static SKColor LightGray = new(196, 196, 196);

        public readonly static SKColor PanelBackgroundDark = new(0x1d, 0x1f, 0x24);
        public readonly static SKColor PanelTitleBar = new(0x2d, 0x30, 0x3d);
        public readonly static SKColor PanelShadow = new(0x10, 0x10, 0x10);

        /// <summary>create a new color with given color</summary>
        public static SKColor SetAlpha(this SKColor color, byte alpha)
        {
            return new SKColor(color.Red, color.Green, color.Blue, alpha);
        }
    }



    /// <summary>虚拟画布</summary>
    public class DurerCanvas
    {
        public readonly DurerCanvas? parent;
        public readonly DurerCoordinateSystem coord;
        public readonly SKSurface surface;
        public readonly SKCanvas canvas;

        public bool IsRoot => parent == null;

        public DurerCanvas(int width, int height, SKColorType colorType = SKColorType.Rgba8888)
        {
            parent = null;
            coord = new DurerCoordinateSystem(new SKSize(width, height));
            surface = SKSurface.Create(new SKImageInfo(width, height, colorType));
            canvas = surface.Canvas;
        }
        protected DurerCanvas(DurerCanvas parent, SKPoint pos, SKSize size, SKPoint origin, SKPoint scale)
        {
            this.parent = parent;
            coord = parent.coord.SubCoordinates(pos, size, origin, scale);
            surface = parent.surface;
            canvas = surface.Canvas;
        }

        public DurerCanvas SubCanvas(
            SKPoint pos,
            SKSize size,
            SKPoint origin,
            SKPoint scale
        ){
            return new DurerCanvas(this, pos, size, origin, scale);
        }

        /// <summary>创建一个渐隐Shader</summary>
        public SKShader CreateFadeShader(SKColor backgroundColor, SKColor color, float fadeDst = 0.15f)
        {
            return DurerShaderUtils.FadeShader(
                coord.deviceSize.Width, coord.deviceSize.Height,
                backgroundColor, color, fadeDst
            );
        }

        /// <summary>绘制背景</summary>
        public void DrawBackground(SKColor color)
        {
            var lt = coord.deviceLT;
            var size = coord.deviceSize;

            Console.WriteLine(lt);
            Console.WriteLine(size);

            canvas.DrawRect(
                lt.X, lt.Y, size.Width, size.Height,
                new SKPaint { 
                    Color = color, 
                    Style = SKPaintStyle.Fill 
                }
            );
        }

        public void DrawPath(SKPath path, SKPaint paint)
        {
            canvas.SetMatrix(coord.mathToRoot.PostConcat(coord.rootToDevice));
            canvas.DrawPath(path, paint);
            canvas.ResetMatrix();
        }

        /// <summary>绘制线段</summary>
        public void DrawLineRaw(float devx, float devy, float devx2, float devy2, SKColor color, float lineWidth, SKPathEffect? effect = null)
        {
            var paint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                PathEffect = effect
            };
            canvas.DrawLine(devx, devy, devx2, devy2, paint);
        }

        /// <summary>绘制线段</summary>
        public void DrawLine(
            float x, 
            float y, 
            float x2, 
            float y2, 
            SKColor color, 
            float lineWidth,
            SKPathEffect? effect = null
        ){
            var pt = coord.MathToDevice(x, y);
            var pt2 = coord.MathToDevice(x2, y2);
            DrawLineRaw(pt.X, pt.Y, pt2.X, pt2.Y, color, lineWidth, effect);
        }

        public void DrawLabel(
            float x, 
            float y, 
            string text, 
            SKColor textColor = default,
            SKColor backgroundColor = default,
            float fontSize = 16,
            string fontFamily = "Arial",
            TextAlignment align = TextAlignment.Center,
            bool isItalic = false,
            int fontWeight = 1,
            SKPoint anchor = default,
            SKPoint offset = default
        ){
            var pt = coord.MathToDevice(x, y);
            canvas.DurerDrawLabel(
                pt.X, pt.Y, text,
                textColor,
                backgroundColor,
                fontSize,
                fontFamily,
                align,
                isItalic,
                fontWeight,
                anchor,
                offset
            );
        }

        public void DrawPanelRaw(
            float devX, float devY, float devW, float devH,
            SKColor backgroundColor,
            SKColor shadowColor,
            float borderRadius = 10.0f,
            float shadowOffsetX = 5.0f,
            float shadowOffsetY = 5.0f,
            float shadowBlur = 5.0f,
            SKShader? shader = null
        )
        {
            // 绘制阴影
            var shadowPaint = new SKPaint
            {
                Color = shadowColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, shadowBlur),
                Shader = shader
            };
            canvas.DrawRoundRect(
                devX - shadowOffsetX,
                devY - shadowOffsetY,
                devW + shadowOffsetX * 2,
                devH + shadowOffsetY * 2,
                borderRadius, borderRadius,
                shadowPaint
            );

            // 绘制面板
            var paint = new SKPaint
            {
                Color = backgroundColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Shader = shader
            };
            canvas.DrawRoundRect(devX, devY, devW, devH, borderRadius, borderRadius, paint);
        }



        public void DrawPanel(
            float x, float y, float w, float h, 
            SKColor backgroundColor, 
            SKColor shadowColor,
            float borderRadius = 10.0f,
            float shadowOffsetX = 5.0f, 
            float shadowOffsetY = 5.0f,
            float shadowBlur = 5.0f,
            SKShader? shader = null
        ){
            var size = coord.ToDeviceSize(w, h);
            var pos = coord.MathToDevice(x, y + h);

            DrawPanelRaw(
                pos.X, pos.Y, size.Width, size.Height,
                backgroundColor, shadowColor,
                borderRadius, shadowOffsetX, shadowOffsetY, shadowBlur,
                shader
            );
        }

        public void DrawPanelOfCanvas(
            SKColor backgroundColor,
            SKColor shadowColor,
            float borderRadius = 10.0f,
            float shadowOffsetX = 5.0f, 
            float shadowOffsetY = 5.0f,
            float shadowBlur = 5.0f,
            SKShader? shader = null
        ){
            var size = coord.deviceSize;
            var pos = coord.deviceLT;

            DrawPanelRaw(
                pos.X, pos.Y, size.Width, size.Height,
                backgroundColor, shadowColor,
                borderRadius, shadowOffsetX, shadowOffsetY, shadowBlur,
                shader
            );
        }

        /// <summary>绘制背景网格，也可以理解为坐标系网格</summary>
        /// <param name="devX">网格左下角x坐标</param>
        /// <param name="devY">网格左下角角y坐标</param>
        /// <param name="devW">网格宽度</param>
        /// <param name="devH">网格高度</param>
        /// <param name="originX">坐标系原点x坐标</param>
        /// <param name="originY">坐标系原点y坐标</param>
        /// <param name="devIntervalW">x轴网格间隔</param>
        /// <param name="devIntervalH">y轴网格间隔</param>
        /// <param name="lineColor">网格线颜色</param>
        /// <param name="lineWidth">网格线宽度</param>
        /// <param name="shader">渐变着色器</param>
        public void DrawGridsRaw(
            float devX, float devY,
            float devOriginX, float devOriginY,
            float devW, float devH,
            float devIntervalW, float devIntervalH,
            float lineWidth,
            SKColor lineColor,
            SKShader? shader = null
        ){
            if( devIntervalW * devIntervalH == 0 ) return;
            devIntervalW = Math.Abs(devIntervalW);
            devIntervalH = Math.Abs(devIntervalH);
            var paint = new SKPaint
            {
                Color = lineColor,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                Shader = shader
            };
            
            float travelX = devOriginX;
            float left = devX;
            float right = devX + devW;

            float yFrom = devY;
            float yTo = devY - devH;

            while( travelX < right )
            {
                canvas.DrawLine(travelX, yFrom, travelX, yTo, paint);
                travelX += devIntervalW;
            }
            travelX = devOriginX;
            while( travelX > left )
            {
                canvas.DrawLine(travelX, yFrom, travelX, yTo, paint);
                travelX -= devIntervalW;
            }

            float travelY = devOriginY;
            float top = devY - devH;
            float bottom = devY;

            float xFrom = devX;
            float xTo = devX + devW;

            while( travelY < bottom )
            {
                canvas.DrawLine(xFrom, travelY, xTo, travelY, paint);
                travelY += devIntervalH;
            }
            travelY = devOriginY;
            while( travelY > top )
            {
                canvas.DrawLine(xFrom, travelY, xTo, travelY, paint);
                travelY -= devIntervalH;
            }
        }

        public void DrawGrids(
            float x, float y,
            float ox, float oy,
            float w, float h,
            float intervalX, float intervalY,
            float lineWidth,
            SKColor lineColor,
            SKShader? shader = null
        ){
            var pt = coord.MathToDevice(x, y);
            var size = coord.ToDeviceSize(w, h);
            var origin = coord.MathToDevice(ox, oy);
            var interval = coord.ToDeviceSize(intervalX, intervalY);

            DrawGridsRaw(
                pt.X, pt.Y, origin.X, origin.Y, 
                size.Width, size.Height, 
                interval.Width, interval.Height, 
                lineWidth, lineColor, shader
            );
        }

        public void DrawGrids(
            float x,
            float y,
            float w,
            float h,
            float intervalX,
            float intervalY,
            float lineWidth,
            SKColor lineColor,
            SKShader? shader = null
        ){
            var pt = coord.MathToDevice(x, y);
            var size = coord.ToDeviceSize(w, h);
            var origin = coord.MathToDevice(0, 0);
            var interval = coord.ToDeviceSize(intervalX, intervalY);

            DrawGridsRaw(
                pt.X, pt.Y, origin.X, origin.Y, 
                size.Width, size.Height, 
                interval.Width, interval.Height, 
                lineWidth, lineColor, shader
            );
        }

        
        public void DrawGridsOfCanvas(
            float gridW,
            float gridH,
            float lineWidth,
            SKColor lineColor,
            SKShader? shader = null
        ){
            var interval = coord.ToDeviceSize(gridW, gridH);
            var lt = coord.deviceLB;
            var origin = coord.MathToDevice(0, 0);
            var size = coord.deviceSize;
            DrawGridsRaw(
                lt.X, lt.Y, origin.X, origin.Y,
                size.Width, size.Height,
                interval.Width, interval.Height,
                lineWidth,
                lineColor,
                shader
            );
        }



        /// <summary>绘制数轴</summary>
        public void DrawAxisRaw(
            SKColor color, float lineWidth, SKShader? shader = null
        ){
            var paint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                Shader = shader
            };
            var origin = coord.MathToDevice(0, 0);
            float left = coord.deviceLB.X;
            float right = coord.deviceLB.X + coord.Width;
            canvas.DrawLine(left, origin.Y, right, origin.Y, paint);

            float top = coord.deviceLB.Y - coord.Height;
            float bottom = coord.deviceLB.Y;
            canvas.DrawLine(origin.X, top, origin.X, bottom, paint);
        }

        public void DrawAxisLabels(
            float intervalX,
            float intervalY,
            SKColor textColor = default,
            SKColor backgroundColor = default,
            float fontSize = 16,
            string fontFamily = "Arial",
            TextAlignment align = TextAlignment.Center,
            bool isItalic = false,
            int fontWeight = 1,
            SKPoint anchor = default,
            SKPoint offset = default
        ){
            var interval = coord.ToDeviceSize(intervalX, intervalY);
            var origin = coord.MathToDevice(0, 0);
            float left = coord.deviceLB.X;
            float right = coord.deviceLB.X + coord.Width;

            float travelX = origin.X;
            float value = 0;
            while(travelX > left)
            {
                canvas.DurerDrawLabel(
                    travelX, origin.Y, ((int)value).ToString(),
                    textColor: textColor,
                    backgroundColor: backgroundColor,
                    fontSize: fontSize,
                    fontFamily: fontFamily,
                    align: align,
                    isItalic: isItalic,
                    fontWeight: fontWeight,
                    anchor: anchor,
                    offset: offset
                );
                value -= intervalX;
                travelX -= interval.Width;
            }

            travelX = origin.X + interval.Width;
            value = intervalX;
            while(travelX < right)
            {
                canvas.DurerDrawLabel(
                    travelX, origin.Y, ((int)value).ToString(),
                    textColor: textColor,
                    backgroundColor: backgroundColor,
                    fontSize: fontSize,
                    fontFamily: fontFamily,
                    align: align,
                    isItalic: isItalic,
                    fontWeight: fontWeight,
                    anchor: anchor,
                    offset: offset
                );
                value += intervalX;
                travelX += interval.Width;
            }

            float top = coord.deviceLB.Y - coord.Height;
            float bottom = coord.deviceLB.Y;

            float travelY = origin.Y;
            value = 0;
            while(travelY < bottom)
            {
                canvas.DurerDrawLabel(
                    origin.X, travelY, ((int)value).ToString(),
                    textColor: textColor,
                    backgroundColor: backgroundColor,
                    fontSize: fontSize,
                    fontFamily: fontFamily,
                    align: align,
                    isItalic: isItalic,
                    fontWeight: fontWeight,
                    anchor: anchor,
                    offset: offset
                );
                value -= intervalY;
                travelY += interval.Height;
            }

            travelY = origin.Y - interval.Height;
            value = intervalY;
            while(travelY > top)
            {
                canvas.DurerDrawLabel(
                    origin.X, travelY, ((int)value).ToString(),
                    textColor: textColor,
                    backgroundColor: backgroundColor,
                    fontSize: fontSize,
                    fontFamily: fontFamily,
                    align: align,
                    isItalic: isItalic,
                    fontWeight: fontWeight,
                    anchor: anchor,
                    offset: offset
                );
                value += intervalY;
                travelY -= interval.Height;
            }
        }

        public void DrawPointsRaw(
            SKPoint[] points,
            SKColor color,
            float radius,
            SKShader? shader = null
        ){
            if(points == null || points.Length == 0) return;
            var paint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Shader = shader
            };
            foreach(var pt in points)
                canvas.DrawCircle(pt, radius, paint);
        }


        public void DrawPathLineRaw(
            SKPoint[] points,
            SKColor color,
            float lineWidth,
            SKShader? shader = null
        ){
            if(points == null || points.Length == 0) return;
            var paint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                Shader = shader
            };
            var path = new SKPath();            
            path.MoveTo(points[0]);
            for(int i = 1; i < points.Length; i++)
                path.LineTo(points[i]);
            canvas.DrawPath(path, paint);
        }

        public void DrawPathLine(
            SKPoint[] points,
            SKColor color,
            float lineWidth,
            SKShader? shader = null
        ){
            var devPoints = coord.MathToDevice(points);
            DrawPathLineRaw(devPoints, color, lineWidth, shader);
        }

        public void DrawPoints(
            SKPoint[] points,
            SKColor color,
            float radius,
            SKShader? shader = null
        ){
            var devPoints = coord.MathToDevice(points);
            DrawPointsRaw(devPoints, color, radius, shader);
        }

        /// <summary>绘制制定的函数</summary>
        /// <param name="f">函数</param>
        /// <param name="left">x轴左边界</param>
        /// <param name="right">x轴右边界</param>
        /// <param name="bottom">y轴下边界</param>
        /// <param name="top">y轴上边界</param>
        /// <param name="color">线条颜色</param>
        /// <param name="lineWidth">线条宽度</param>
        /// <param name="shader">着色器</param>
        public void DrawFunction(
            Func<float, float> f, 
            float left, 
            float right, 
            float bottom, 
            float top, 
            SKColor color,
            float lineWidth,
            SKShader? shader = null
        ){
            foreach(var pts in DurerMath.InvokeFunction(f, left, right, bottom, top))
            {
                var validPts = new List<SKPoint>();
                foreach(var pt in pts)
                {
                    if (float.IsNaN(pt.Y) || float.IsInfinity(pt.Y)) continue;
                    validPts.Add(pt);
                }
                DrawPathLine(validPts.ToArray(), color, lineWidth, shader);
            }
        }

        /// <summary>绘制制定的函数(以当前画布范围为准)</summary>
        public void DrawFunction(
            Func<float, float> f,
            SKColor color,
            float lineWidth,
            SKShader? shader = null
        ){
            var mathLB = coord.DeviceToMath(coord.deviceLB);
            var size = coord.ToMathSize(coord.deviceSize);
            var mathRT = new SKPoint(mathLB.X + size.Width, mathLB.Y + size.Height);
            DrawFunction(f, mathLB.X, mathRT.X, mathLB.Y, mathRT.Y, color, lineWidth, shader);
        }

        /// <summary>绘制制定的函数(以当前画布范围为准)</summary>
        public void DrawFunctionFade(
            Func<float, float> f,
            SKColor color,
            SKColor backgroundColor,
            float lineWidth = 2.5f,
            float fadeDst = 0.15f
        ){
            var shader = CreateFadeShader(backgroundColor, color, fadeDst);
            DrawFunction(f, color, lineWidth, shader);
        }   


        /// <summary>绘制函数在某个点的切线函数</summary>
        public void DrawFunctionTangentLine(
            Func<float, float> f, float x,
            SKColor color,
            float lineWidth,
            SKShader? shader = null
        ){
            /* 求函数在目标点的切线函数 */
            var pt0 = new SKPoint(x, f(x));
            if(float.IsNaN(pt0.Y) || float.IsInfinity(pt0.Y))return;

            float slope = DurerMath.Derivative(f, x);
            if(float.IsNaN(slope) || float.IsInfinity(slope))return;

            float bias = pt0.Y - slope * pt0.X;
            var tangentFunc = new Func<float, float>(x => slope * x + bias);

            /* 绘制函数 */
            DrawFunction(tangentFunc, color, lineWidth, shader);
        }

        /// <summary>绘制函数在某个点的切线函数</summary>
        public void DrawFunctionTangentLineFade(
            Func<float, float> f, float x,
            SKColor color,
            float lineWidth,
            SKColor backgroundColor
        ){
            var shader = CreateFadeShader(backgroundColor, color);
            DrawFunctionTangentLine(f, x, color, lineWidth, shader);
        }


        /// <summary>在指定的位置绘制目标图形</summary>
        /// <param name="shape">目标图形</param>
        /// <param name="pos">位置</param>
        /// <param name="color">颜色</param>
        public void DrawShape(
            DurerShape shape,
            SKPoint pos,
            SKColor color = default
        ){
            var path = shape.GenPath(pos);
            DrawPath(path, new SKPaint(){
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = color.GetColorOrDefault(Colors.Dark)
            });
        }


        /// <summary>在给定的一组点绘制目标图形</summary>
        public void DrawShapes(
            DurerShape shape, 
            SKPoint[] points,
            SKColor color = default
        ){
            var pathes = points.Select(shape.GenPath);
            canvas.SetMatrix(coord.mathToDevice);
            foreach(var path in pathes)
            {
                DrawPath(path, new SKPaint(){
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = color.GetColorOrDefault(Colors.Dark)
                });
            }
            canvas.ResetMatrix();
        }


        /// <summary>绘制箭头</summary>
        /// <param name="pos">箭头位置</param>
        /// <param name="color">箭头颜色</param>
        /// <param name="rotation">箭头旋转角度</param>
        /// <param name="length">箭头长度</param>
        /// <param name="angle">箭头角度</param>
        public void DrawArrow(
            SKPoint pos,
            SKColor color = default,
            float rotation = 0,
            float length = 0.12f,
            float angle = 0.69812477f
        ){
            var arrow = new DurerArrow(length, angle);
            arrow.RotateDeg(rotation);
            var path = arrow.GenPath(pos);
            DrawPath(path, new SKPaint(){
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = color.GetColorOrDefault(Colors.Dark)
            });
        }

        /// <summary>绘制贝塞尔曲线</summary>
        public void DrawBezierCurve(
            SKPoint pos, 
            SKPoint control1,
            SKPoint control2,
            SKPoint end,
            SKColor color = default,
            float lineWidth = 1.5f
        ){
            pos = coord.MathToDevice(pos);
            control1 = coord.MathToDevice(control1);
            control2 = coord.MathToDevice(control2);
            end = coord.MathToDevice(end);
            
            var path = new SKPath();
            path.MoveTo(pos);
            path.CubicTo(control1, control2, end);
            canvas.DrawPath(path, new SKPaint(){
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = color.GetColorOrDefault(Colors.Dark),
                StrokeWidth = lineWidth,
                StrokeCap = SKStrokeCap.Round,
            });
        }

        /// <summary>绘制贝塞尔曲线</summary>
        public void DrawBezierCurveArrow(
            SKPoint start,
            SKPoint end,
            SKColor color = default,
            SKColor arrowColor = default,
            float arrowSize = 0.12f,
            float lineWidth = 1.5f
        ){
            float center = (end.X + start.X) * 0.5f;
            var control1 = new SKPoint(center, start.Y);
            var control2 = new SKPoint(center, end.Y);

            /* 绘制贝塞尔曲线 */
            var startDevice = coord.MathToDevice(start);
            var endDevice = coord.MathToDevice(end);
            var control1Device = coord.MathToDevice(control1);
            var control2Device = coord.MathToDevice(control2);
            var path = new SKPath();
            path.MoveTo(startDevice);
            path.CubicTo(control1Device, control2Device, endDevice);
            canvas.DrawPath(path, new SKPaint(){
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = color.GetColorOrDefault(Colors.Dark),
                StrokeWidth = lineWidth,
                StrokeCap = SKStrokeCap.Round,
            });

            /* 绘制尾部的箭头 */
            var arrow = new DurerArrow(arrowSize);
            float theta = DurerUtils.GetAngleOfBezierCurve(start, control1, control2, end);
            arrow.RotateRad(theta - MathF.PI * 0.5f);
            var pathArrow = arrow.GenPath(end);
            DrawPath(pathArrow, new SKPaint(){
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = arrowColor.GetColorOrDefault(Colors.Dark)
            });
        }


        public void DrawLineSegment(
            float x0, 
            float y0,
            float x1, 
            float y1,
            SKColor lineColor,
            float lineWidth,
            float endpointRadius,
            SKColor endpointColor,
            SKPathEffect? pathEffect = null,
            SKShader? shader = null
        ){
            var devStart = coord.MathToDevice(x0, y0);
            var devEnd = coord.MathToDevice(x1, y1);
            canvas.DurerDrawLineSegment(
                devStart.X, 
                devStart.Y, 
                devEnd.X, 
                devEnd.Y,
                lineColor, 
                lineWidth, 
                endpointRadius, 
                endpointColor, 
                pathEffect, shader
            );
        }

        /// <summary>绘制圆点标记</summary>
        public void DrawCircleMark(
            float x, 
            float y, 
            float ptRadius, 
            SKColor ptColor,
            float outerRadius,
            SKColor outerColor,
            float outerLineWidth
        ){
            var devPt = coord.MathToDevice(x, y);
            canvas.DrawCircleMark(
                devPt.X, 
                devPt.Y, 
                ptRadius, 
                ptColor, 
                outerRadius, 
                outerColor, 
                outerLineWidth
            );
        }

        public void SavePng(string filepath, int quality = 100)
        {
            canvas.Flush();
            surface.Snapshot().SavePng(filepath, quality);
        }
    }
}