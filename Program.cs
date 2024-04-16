using Durer;
using SkiaSharp;

public class Program
{
    public static void Main(string[] args)
    {
        Example2().SavePng("./output.png");
    }
    public static DurerCanvas Example2()
    {
        var style = DurerStyle.Default;
        style.fontStyle.fontFamily = "Fira Code";
        style.fontStyle.fontWeight = 2;

        var size = new SKSizeI(1440, 940);
        var origin = new SKPointI(700, 350);
        var padding = new SKPointI(20, 20);
        var mathCanvas = DurerHelper.CreateMathCanvas(size, origin, 140, 4, 4, padding, style);

        // var points = DurerUtils.GenRandomPoints(new SKPoint(-1, 1), new SKPoint(2, 4), 10);
        // var arrow = new DurerArrow(0.15f, MathF.PI * 0.333333f);
        // mathCanvas.DrawShapes(
        //     arrow,
        //     points,
        //     SKColor.Parse("#a70055")
        // );

        // points = DurerUtils.GenRandomPoints(new SKPoint(2, -2), new SKPoint(5, 1), 10);
        // var square = new DurerSquare(0.2f);
        // mathCanvas.DrawShapes(
        //     square,
        //     points
        // );
        // var dotEffects = SKPathEffect.CreateDash(new float[]{10, 10}, 0);
        // mathCanvas.DrawLine(2, 2, 2, 0.7f, style.mathStyle.lineSegmentColor, style.mathStyle.lineSegmentWidth, dotEffects);

        // var arrow = new DurerArrow(0.15f, MathF.PI * 0.333333f);
        // mathCanvas.DrawShape(arrow, new SKPoint(2, 2), SKColor.Parse("#a70055"));
        // mathCanvas.DrawLabel(2, 2, "(x1, y1)", style, new SKPoint(0, 0.5f), new SKPoint(25, 0));
        

        // mathCanvas.DrawCircleMarkWithLabel(2, 0.7f, "(x1, y)", style, 5);

        // mathCanvas.DrawFunctionFade(x => 0.35f * x, style);
        // Func<float, float> sigmond = x => 1 / (1 + MathF.Exp(-x));

        // Func<float, float> step = x => MathF.Max(0, MathF.Min(1, x));
        // Func<float, float> func = x => MathF.Abs(x);

        var offset = new SKPoint(0.2f, 0.2f);
        var pt = new SKPoint(0.8f, 0.23f);
        var list = new List<Func<float, float>>();
        for(float i = -0.5f; i <= 0.5f; i += 0.1f)
        {
            list.Add(DurerMath.GetLinearFunc(i, pt));
            pt += offset;
        }


        // var funcs = new Func<float, float>[]{
        //     DurerMath.GetLinearFunc(-0.1f, new SKPoint(0.29f, 0.24f)),
        //     DurerMath.GetLinearFunc(-0.2f, new SKPoint(0.28f, 0.25f)),
        //     DurerMath.GetLinearFunc(-0.3f, new SKPoint(0.27f, 0.26f)),
        //     DurerMath.GetLinearFunc(-0.4f, new SKPoint(0.26f, 0.27f)),
        //     DurerMath.GetLinearFunc(-0.5f, new SKPoint(0.25f, 0.28f)),
        //     DurerMath.GetLinearFunc(0, pt),
        //     DurerMath.GetLinearFunc(0.1f, new SKPoint(0.31f, 0.23f)),
        //     DurerMath.GetLinearFunc(0.2f, new SKPoint(0.32f, 0.22f)),
        //     DurerMath.GetLinearFunc(0.3f, new SKPoint(0.33f, 0.21f)),
        //     DurerMath.GetLinearFunc(0.4f, new SKPoint(0.34f, 0.20f)),
        //     DurerMath.GetLinearFunc(0.5f, new SKPoint(0.35f, 0.19f)),
        // };

        foreach(var func in list){
            mathCanvas.DrawFunctionFade(func, style);
        }
        // mathCanvas.DrawLabel(0, 0.5f, "\"Sigmond\"", style, new SKPoint(0.5f, 0.5f), new SKPoint(0, 30));
        return mathCanvas;
    }
}