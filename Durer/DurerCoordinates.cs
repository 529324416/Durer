using SkiaSharp;

namespace Durer
{
    /// <summary>图勒坐标系系统</summary>
    public class DurerCoordinateSystem
    {
        public readonly SKSize size;
        public readonly SKPoint position;
        public readonly SKPoint origin;
        public readonly SKPoint scale;

        public float Width => size.Width;
        public float Height => size.Height;
        public float X => position.X;
        public float Y => position.Y;
        public float OriginX => origin.X;
        public float OriginY => origin.Y;
        public float ScaleX => scale.X;
        public float ScaleY => scale.Y;

        public float MathX => position.X - origin.X;
        public float MathY => position.Y - origin.Y;

        /// <summary>原始坐标:当前画布的左上角</summary>
        public readonly SKPoint deviceLT;
        public readonly SKPoint deviceLB;
        public readonly SKSize deviceSize;

        public readonly SKMatrix rootToDevice;
        public readonly SKMatrix deviceToRoot;
        public readonly SKMatrix mathToRoot;
        public readonly SKMatrix rootToMath;
        public readonly SKMatrix mathToDevice;

        public DurerCoordinateSystem(
            SKSize size
        ){
            this.size = size;
            this.position = new SKPoint(0, 0);
            this.origin = new SKPoint(0, 0);
            this.scale = new SKPoint(1, 1);

            this.deviceLT = new SKPoint(0, 0);
            this.deviceSize = size;
            this.deviceLB = new SKPoint(0, size.Height);

            this.rootToDevice = SKMatrix.CreateScaleTranslation(1, -1, 0, size.Height);
            this.deviceToRoot = rootToDevice.Invert();

            this.mathToRoot = SKMatrix.CreateIdentity();
            this.rootToMath = mathToRoot.Invert();

            this.mathToDevice = mathToRoot.PostConcat(rootToDevice);
        }
        protected DurerCoordinateSystem(
            DurerCoordinateSystem parent,
            SKSize size,
            SKPoint position,
            SKPoint origin,
            SKPoint scale
        ){
            this.size = size;
            this.position = position;
            this.origin = origin;
            this.scale = scale;

            this.rootToDevice = parent.rootToDevice;
            this.deviceToRoot = parent.deviceToRoot;
            this.mathToRoot = SKMatrix
            .CreateScale(scale.X, scale.Y)
            .PostConcat(SKMatrix.CreateTranslation(origin.X, origin.Y))
            .PostConcat(parent.mathToRoot);

            var dsize = parent.ToDeviceSize(size.Width, size.Height);
            this.deviceSize = new SKSize(dsize.Width, dsize.Height);
            this.deviceLB = parent.MathToDevice(position.X, position.Y);
            this.deviceLT = new SKPoint(deviceLB.X, deviceLB.Y - deviceSize.Height);

            this.rootToMath = parent.rootToMath
            .PostConcat(SKMatrix.CreateTranslation(-origin.X, -origin.Y))
            .PostConcat(SKMatrix.CreateScale(1 / scale.X, 1 / scale.Y));

            this.mathToDevice = mathToRoot.PostConcat(rootToDevice);
        }

        /// <summary>子坐标系</summary>
        public DurerCoordinateSystem SubCoordinates(
            SKPoint position,
            SKSize size,
            SKPoint origin,
            SKPoint scale
        ){
            if(scale.X * scale.Y == 0)
                throw new ArgumentException("Scale can not be zero");
            return new DurerCoordinateSystem(this, size, position, origin, scale);
        }

        public SKSize ToDeviceSize(float x, float y)
        {
            var pt = mathToRoot.MapVector(x, y);
            return new SKSize(pt.X, pt.Y);
        }
        public SKSize ToDeviceSize(SKPoint point) => ToDeviceSize(point.X, point.Y);
        public SKSize ToDeviceSize(SKSize size) => ToDeviceSize(size.Width, size.Height);

        public SKSize ToMathSize(float x, float y)
        {
            var pt = rootToMath.MapVector(x, y);
            return new SKSize(pt.X, pt.Y);
        }
        public SKSize ToMathSize(SKPoint point) => ToMathSize(point.X, point.Y);
        public SKSize ToMathSize(SKSize size) => ToMathSize(size.Width, size.Height);

        /// <summary>将给定的坐标转换为父类的数学坐标</summary>
        public SKPoint MathToRoot(SKPoint pt) => mathToRoot.MapPoint(pt);
        public SKPoint[] MathToRoot(SKPoint[] points) => mathToRoot.MapPoints(points);
        public SKPoint MathToRoot(float x, float y) => MathToRoot(new SKPoint(x, y));

        /// <summary>将给定的父类坐标转换为设备坐标</summary>
        public SKPoint RootToDevice(SKPoint pt) => rootToDevice.MapPoint(pt);
        public SKPoint[] RootToDevice(SKPoint[] points) => rootToDevice.MapPoints(points);
        public SKPoint RootToDevice(float x, float y) => RootToDevice(new SKPoint(x, y));

        /// <summary>将给定的数学坐标转换为设备坐标</summary>
        public SKPoint MathToDevice(SKPoint pt) => RootToDevice(MathToRoot(pt));
        public SKPoint[] MathToDevice(SKPoint[] points) => RootToDevice(MathToRoot(points));
        public SKPoint MathToDevice(float x, float y) => MathToDevice(new SKPoint(x, y));

        public SKPoint RootToMath(SKPoint pt) => rootToMath.MapPoint(pt);
        public SKPoint[] RootToMath(SKPoint[] points) => rootToMath.MapPoints(points);
        public SKPoint RootToMath(float x, float y) => RootToMath(new SKPoint(x, y));

        public SKPoint DeviceToRoot(SKPoint pt) => deviceToRoot.MapPoint(pt);
        public SKPoint[] DeviceToRoot(SKPoint[] points) => deviceToRoot.MapPoints(points);
        public SKPoint DeviceToRoot(float x, float y) => DeviceToRoot(new SKPoint(x, y));

        public SKPoint DeviceToMath(SKPoint pt) => RootToMath(DeviceToRoot(pt));
        public SKPoint[] DeviceToMath(SKPoint[] points) => RootToMath(DeviceToRoot(points));
        public SKPoint DeviceToMath(float x, float y) => DeviceToMath(new SKPoint(x, y));
    }


}