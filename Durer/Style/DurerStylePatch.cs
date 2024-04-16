


using SkiaSharp;

namespace Durer
{
    public static class DurerStylePatch
    {
        /// <summary>绘制背景</summary>
        public static void DrawBackground(this DurerCanvas canvasEx, DurerStyle style) =>
            canvasEx.DrawBackground(style.backgroundColor);

        /// <summary>绘制一个带有阴影的面板,通常用于承接其他的元素</summary>
        /// <param name="x">面板的左下角x坐标</param>
        /// <param name="y">面板的左下角y坐标</param>
        /// <param name="w">面板的宽度</param>
        /// <param name="h">面板的高度</param>
        /// <param name="style">风格</param>
        public static void DrawPanel(
            this DurerCanvas canvas,
            float x, float y, float w, float h,
            DurerStyle style
        ) =>
            canvas.DrawPanel(
                x, y, w, h,
                style.backgroundColor,
                style.shadowColor,
                style.borderRadius,
                style.shadowOffset.X,
                style.shadowOffset.Y,
                style.shadowBlur
            );

        public static void DrawLabel(
            this DurerCanvas canvas,
            float x,
            float y,
            string text,
            DurerStyle style,
            SKPoint anchor = default,
            SKPoint offset = default
        ) =>
            canvas.DrawLabel(
                x, y, text,
                style.fontStyle.fontColor,
                style.fontStyle.backgroundColor,
                style.fontStyle.fontSize,
                style.fontStyle.fontFamily,
                style.fontStyle.textAlignment,
                style.fontStyle.isItalic,
                style.fontStyle.fontWeight,
                anchor,
                offset
            );

        /// <summary>绘制一个带有阴影的面板,通常用于承接其他的元素</summary>
        /// <param name="style">风格</param>
        public static void DrawPanelOfCanvas(this DurerCanvas canvas, DurerStyle style) =>
            canvas.DrawPanelOfCanvas(
                style.backgroundColor,
                style.shadowColor,
                style.borderRadius,
                style.shadowOffset.X,
                style.shadowOffset.Y,
                style.shadowBlur
            );

        /// <summary>绘制函数曲线，但是追加边缘淡出效果</summary>
        /// <param name="func">函数</param>
        /// <param name="style">风格</param>
        /// <param name="funcLineColor">线条颜色</param>
        public static void DrawFunctionFade(
            this DurerCanvas canvas, 
            Func<float, float> func,
            DurerStyle style,
            SKColor funcLineColor = default
        ){
            canvas.DrawFunctionFade(
                func, 
                ColorUtils.GetColorOrDefault(funcLineColor, style.mathStyle.lineColor),
                style.backgroundColor,
                style.mathStyle.lineWidth,
                style.fadeRatio
            );
        }

        public static void DrawLineSegment(
            this DurerCanvas canvas, 
            float x0, float y0, 
            float x1, float y1,
            DurerStyle style,
            SKPathEffect? effect = null,
            SKShader? shader = null
        ){
            canvas.DrawLineSegment(
                x0, y0, x1, y1,
                style.mathStyle.lineSegmentColor,
                style.mathStyle.lineSegmentWidth,
                style.mathStyle.lineSegmentEndpointRadius,
                style.mathStyle.lineSegmentEndpointColor,
                effect,
                shader
            );
        }

        public static void DrawCircleMark(
            this DurerCanvas canvas,
            float x,
            float y,
            DurerStyle style
        ){
            canvas.DrawCircleMark(
                x, 
                y,
                style.mathStyle.markPointRadius,
                style.mathStyle.markPointColor,
                style.mathStyle.markPointCircleRadius,
                style.mathStyle.markPointCircleColor,
                style.mathStyle.markPointCircleWidth
            );
        }

        public static void DrawCircleMarkWithLabel(
            this DurerCanvas canvas,
            float x,
            float y,
            string label,
            DurerStyle style,
            float padding = 5
        ){
            canvas.DrawCircleMark(x, y, style);
            var offset = new SKPoint(style.mathStyle.markPointCircleRadius + padding, 0);
            canvas.DrawLabel(x, y, label, style, new SKPoint(0f, 0.5f), offset);
        }
    }
}