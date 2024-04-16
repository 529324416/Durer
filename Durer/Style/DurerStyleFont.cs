using SkiaSharp;
using Topten.RichTextKit;

namespace Durer
{
    public struct DurerStyleFont
    {
        public static DurerStyleFont Default
        {
            get{
                return new DurerStyleFont(
                    "Fira Code",
                    20,
                    new SKColor(0x1f, 0x20, 0x2d),
                    SKColors.Transparent,
                    2,
                    false,
                    TextAlignment.Center
                );
            }   
        }

        public string fontFamily;
        public float fontSize;
        public SKColor fontColor;
        public SKColor backgroundColor;
        public int fontWeight;
        public bool isItalic;
        public TextAlignment textAlignment;

        public DurerStyleFont(
            string fontFamily,
            float fontSize,
            SKColor fontColor,
            SKColor backgroundColor,
            int fontWeight,
            bool isItalic,
            TextAlignment textAlignment
        ){
            this.fontFamily = fontFamily;
            this.fontSize = fontSize;
            this.fontColor = fontColor;
            this.fontWeight = fontWeight;
            this.isItalic = isItalic;
            this.textAlignment = textAlignment;
        }
    }
}