

using SkiaSharp;

namespace Durer
{
    public static class DurerMath
    {
        static IEnumerable<float> XRange(float l, float r, float step)
        {
            (l, r) = (Math.Min(l, r), Math.Max(l, r));

            if(l > 0)
                for (float i = l; i <= r; i += step)
                        yield return i;

            if(r < 0)
                for (float i = l; i <= r; i += step)
                    yield return i;

            for(float i = l; i < 0; i += step)
                yield return i;

            for(float i = 0; i < r; i += step)
                yield return i;
        }

        /// <summary>给定一个函数图像和绘制范围，返回多个点组形成函数的曲线图</summary>
        /// <param name="func">函数</param>
        /// <param name="left">绘制范围左边界</param>
        /// <param name="right">绘制范围右边界</param>
        /// <param name="bottom">绘制范围下边界</param>
        /// <param name="top">绘制范围上边界</param>
        /// <param name="count">绘制点数</param>
        public static IEnumerable<SKPoint[]> InvokeFunction(
            Func<float, float> func, 
            float left, 
            float right, 
            float bottom, 
            float top,
            int count = 10000
        ){
            if(left >= right || count <= 0)yield break;
            float step = (right - left) / count;
            bool isBreak = false;
            var points = new List<SKPoint>();
            foreach(var x in XRange(left, right, step))
            {
                float y = func(x);
                if(!isBreak){
                    if(float.IsNaN(y) || float.IsInfinity(y) || y > top || y < bottom)
                    {
                        // Console.WriteLine($"函数在x={x}处断裂");
                        isBreak = true;
                        points.Add(new SKPoint(x, y));
                        yield return points.ToArray();
                        points.Clear();
                        continue;
                    }
                    points.Add(new SKPoint(x, y));
                }else
                {
                    if(float.IsNaN(y) || float.IsInfinity(y) || y > top || y < bottom)continue;
                    // Console.WriteLine($"函数在x={x}处恢复");
                    isBreak = false;
                    points.Add(new SKPoint(x, y));
                }
            }
            yield return points.ToArray();
        }

        /// <summary>返回一个线性函数，给该函数的斜率和其穿过的点</summary>
        public static Func<float, float> GetLinearFunc(float slope, SKPoint pt)
        {
            return x => slope * (x - pt.X) + pt.Y;
        }

        /// <summary>计算函数在某一点的导数</summary>
        public static float Derivative(Func<float, float> f, float x, float dx = 0.001f)
        {
            var pt0 = new SKPoint(x - dx, f(x - dx));
            var pt1 = new SKPoint(x + dx, f(x + dx));
            if(float.IsNaN(pt0.Y) || float.IsInfinity(pt0.Y))return float.NaN;
            if(float.IsNaN(pt1.Y) || float.IsInfinity(pt1.Y))return float.NaN;
            return (pt1.Y - pt0.Y) / (dx * 2);
        }

        /// <summary>计算函数在某一点的切线函数</summary>
        public static Func<float, float>? GetTangentFunc(Func<float, float> f, float x)
        {
            /* 求函数在目标点的切线函数 */
            var pt0 = new SKPoint(x, f(x));
            if(float.IsNaN(pt0.Y) || float.IsInfinity(pt0.Y))return null;

            float slope = Derivative(f, x);
            if(float.IsNaN(slope) || float.IsInfinity(slope))return null;

            float bias = pt0.Y - slope * pt0.X;
            return x => slope * x + bias;
        }
    }
}