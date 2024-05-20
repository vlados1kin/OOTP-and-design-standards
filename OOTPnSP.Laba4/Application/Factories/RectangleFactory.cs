using Application.Figures;
using PluginBase;

namespace Application.Factories;

public class RectangleFactory(double x, double y, double bottomSide, double leftSide) : Factory(x, y, bottomSide, leftSide, Math.PI / 2)
{
    public override string TypeOfFactory() => "rectangle";
    public override string NameOfFactory() => "Треугольник";
    public override Figure Create() => new Rectangle(X, Y, BottomSide, LeftSide);
    
    public override string Draw(Figure figure)
    {
        var x1 = Math.Round(figure.X);
        var y1 = Math.Round(figure.Y);
        var x2 = Math.Round(x1 + figure.BottomSide);
        var y2 = y1;
        var x3 = Math.Round(x2 + figure.LeftSide * Math.Cos(Angle));
        var y3 = Math.Round(y1 - LeftSide * Math.Sin(Angle));
        var x4 = Math.Round(x1 + LeftSide * Math.Cos(Angle));
        var y4 = y3;
        return $"M{x1} {y1} L{x2} {y2} L{x3} {y3} L{x4} {y4} L{x1} {y1}";
    }
}