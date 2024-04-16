using SkiaSharp;

namespace Durer
{
    /// <summary>用于绘制数学元素的风格</summary>
    public struct DurerStyleMath
    {
        public static DurerStyleMath Default
        {
            get{
                return new DurerStyleMath(
                    new SKColor(0xe8, 0xe8, 0xe8),
                    1,
                    new SKColor(0xd8, 0xd8, 0xe8),
                    1,
                    new SKColor(0xa8, 0xa8, 0xa8),
                    2,
                    new SKColor(0x00, 0x2F, 0xA7),
                    2.5f,
                    new SKColor(0x1f, 0x20, 0x2d),
                    3f,
                    new SKColor(0xb1, 0x3c, 0x45),
                    5,
                    new SKColor(0x3e, 0x3f, 0x4c),
                    5,
                    new SKColor(0x1f, 0x20, 0x2d),
                    25,
                    1.5f
                );
            }   
        }

        #region Grids
            /// <summary>网格线颜色</summary>
            public SKColor gridColor;

            /// <summary>网格线宽度</summary>
            public float gridWidth;

            /// <summary>大网格线色彩</summary>
            public SKColor gridColorLarge;

            /// <summary>大网格线宽度</summary>
            public float gridWidthLarge;

            public float axisWidth;

            public SKColor axisColor;
        #endregion

        #region LineSegment
            /// <summary>直线颜色</summary>
            public SKColor lineColor;

            /// <summary>直线宽度</summary>
            public float lineWidth;

            /// <summary>线段端点色彩</summary>
            public SKColor lineSegmentColor;

            /// <summary>线段端点半径</summary>
            public float lineSegmentWidth;

            /// <summary>线段端点色彩</summary>
            public SKColor lineSegmentEndpointColor;

            /// <summary>线段端点半径</summary>
            public float lineSegmentEndpointRadius;
        #endregion

        #region MarkPoint
            public SKColor markPointColor;
            public float markPointRadius;
            public SKColor markPointCircleColor;
            public float markPointCircleRadius;
            public float markPointCircleWidth;
        #endregion

        public DurerStyleMath(
            SKColor gridColor,
            float gridWidth,
            SKColor gridColorLarge,
            float gridWidthLarge,
            SKColor axisColor,
            float axisWidth,
            SKColor lineColor,
            float lineWidth,
            SKColor lineSegmentColor,
            float lineSegmentWidth,
            SKColor endpointColor,
            float endpointRadius,
            SKColor markPointColor,
            float markPointRadius,
            SKColor markPointCircleColor,
            float markPointCircleRadius,
            float markPointCircleWidth
        ){
            this.gridColor = gridColor;
            this.gridWidth = gridWidth;
            this.gridColorLarge = gridColorLarge;
            this.gridWidthLarge = gridWidthLarge;
            this.axisColor = axisColor;
            this.axisWidth = axisWidth;

            this.lineColor = lineColor;
            this.lineWidth = lineWidth;

            // 线段
            this.lineSegmentColor = lineSegmentColor;
            this.lineSegmentWidth = lineSegmentWidth;
            this.lineSegmentEndpointColor = endpointColor;
            this.lineSegmentEndpointRadius = endpointRadius;

            // 标记点
            this.markPointColor = markPointColor;
            this.markPointRadius = markPointRadius;
            this.markPointCircleColor = markPointCircleColor;
            this.markPointCircleRadius = markPointCircleRadius;
            this.markPointCircleWidth = markPointCircleWidth;
        }
    }
}