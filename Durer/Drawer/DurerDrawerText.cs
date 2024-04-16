using SkiaSharp;
using Topten.RichTextKit;

namespace Durer
{
    public static partial class DurerDrawer
    {
        /// <param name="canvas">画布</param>
        /// <param name="matrix">坐标变换矩阵</param>
        public static void DurerDrawLabel(
            this SKCanvas canvas,
            float x,
            float y,
            string text,
            SKColor textColor,
            SKColor backgroundColor,
            float fontSize,
            string fontFamily,
            TextAlignment align,
            bool isItalic,
            int fontWeight,
            SKPoint anchor,
            SKPoint offset
        ){
            var richText = new RichString()
            .FontFamily(fontFamily)
            .FontSize(fontSize)
            .TextColor(textColor.GetColorOrDefault(SKColors.Gray))
            .Alignment(align)
            .FontItalic(isItalic)
            .FontWeight(fontWeight)
            .BackgroundColor(backgroundColor.GetColorOrDefault(SKColors.Transparent))
            .Add(text);

            float w = richText.MeasuredWidth;
            float h = richText.MeasuredHeight;
            anchor = new SKPoint(anchor.X, -anchor.Y + 1);

            var pt = new SKPoint(x, y) - new SKPoint(anchor.X * w, anchor.Y * h) + new SKPoint(offset.X, -offset.Y);
            richText.Paint(canvas, pt);
        }
    }
}