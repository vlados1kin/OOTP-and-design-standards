using PluginBase;

namespace Application.Figures;

public class Triangle : Figure
{
    private readonly double _thirdSide;
    public Triangle(double x, double y, double bottomSide, double leftSide, double angle) : base(x, y, bottomSide, leftSide, angle)
    {
        _thirdSide = Math.Sqrt(BottomSide * BottomSide + LeftSide * LeftSide - 2 * BottomSide * LeftSide * Math.Cos(Angle));
    }

    public override double Perimeter() => BottomSide + LeftSide + _thirdSide;
    public override double Area() => 0.5 * BottomSide * LeftSide * Math.Sin(Angle);
}