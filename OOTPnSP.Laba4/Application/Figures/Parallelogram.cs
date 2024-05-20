using PluginBase;

namespace Application.Figures;

public class Parallelogram(double x, double y, double bottomSide, double leftSide, double angle) : Figure(x, y, bottomSide, leftSide, angle)
{
    public override double Area() => BottomSide * LeftSide * Math.Sin(Angle);
    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}