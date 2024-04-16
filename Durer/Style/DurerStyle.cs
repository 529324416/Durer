using SkiaSharp;

namespace Durer
{
    /// <summary>配置类</summary>
    public partial class DurerStyle
    {
        public static DurerStyle Default
        {
            get{
                return new DurerStyle(
                    SKColors.White,
                    10,
                    0.1f,
                    new SKColor(0xd0, 0xd0, 0xd0),
                    new SKPoint(5, 5),
                    5,
                    DurerStyleFont.Default,
                    DurerStyleMath.Default
                );
            }   
        }

        /// <summary>一般背景色彩</summary>
        public SKColor backgroundColor;

        /// <summary>一般圆角半径</summary>
        public float borderRadius;

        /// <summary>渐隐比例，表示距离边缘的多少百分比开始消失(一般为0.1左右)</summary>
        public float fadeRatio;

        #region Shadow
            /// <summary>阴影颜色</summary>
            public SKColor shadowColor;

            /// <summary>阴影偏移</summary>
            public SKPoint shadowOffset;

            /// <summary>阴影模糊度</summary>
            public float shadowBlur;
        #endregion


        public DurerStyleFont fontStyle;
        public DurerStyleMath mathStyle;

        public DurerStyle(
            SKColor backgroundColor,
            float borderRadius,
            float fadeRatio,
            SKColor shadowColor,
            SKPoint shadowOffset,
            float shadowBlur,
            DurerStyleFont fontStyle,
            DurerStyleMath mathStyle
        ){
            this.backgroundColor = backgroundColor;
            this.borderRadius = borderRadius;
            this.fadeRatio = fadeRatio;
            this.shadowColor = shadowColor;
            this.shadowOffset = shadowOffset;
            this.shadowBlur = shadowBlur;
            this.fontStyle = fontStyle;
            this.mathStyle = mathStyle;
        }

        /// <summary>创建一个渐隐Shader</summary>
        public SKShader CreateFadeShader(float Width, float Height, SKColor color) =>
            DurerShaderUtils.FadeShader(Width, Height, backgroundColor, color, fadeRatio);
            
    }




}