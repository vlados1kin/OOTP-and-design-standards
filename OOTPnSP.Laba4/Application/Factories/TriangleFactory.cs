using Application.Figures;
using PluginBase;

namespace Application.Factories;

public class TriangleFactory(double x, double y, double bottomSide, double leftSide, double angle) : Factory(x, y, bottomSide, leftSide, angle)
{
    public override string TypeOfFactory() => "triangle";
    public override string NameOfFactory() => "Треугольник";
    public override Figure Create() => new Triangle(X, Y, BottomSide, LeftSide, Angle);
    public override string Draw(Figure figure)
    {
        var x1 = Math.Round(figure.X);
        var y1 = Math.Round(figure.Y);
        var x2 = Math.Round(x1 + figure.BottomSide);
        var y2 = y1;
        var x3 = Math.Round(x1 + figure.LeftSide * Math.Cos(Angle));
        var y3 = Math.Round(y1 - LeftSide * Math.Sin(Angle));
        return $"M{x1} {y1} L{x2} {y2} L{x3} {y3} L{x1} {y1}";
    }
}