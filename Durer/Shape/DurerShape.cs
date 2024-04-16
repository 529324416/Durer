

using SkiaSharp;

namespace Durer
{
    /// <summary>
    /// Base class for all Durer shapes.
    /// </summary>
    public abstract class DurerShape
    {   
        /// <summary>generate path at target point</summary>
        /// <param name="pt">target point, it must be a math point</param>
        public abstract SKPath GenPath(SKPoint pt);

        /// <summary>rotate the shape by rad radians</summary>
        public abstract void RotateRad(float rad);

        /// <summary>rotate the shape by deg degrees</summary>
        public void RotateDeg(float deg) => RotateRad(deg * 0.01745329277777f);
    }
}