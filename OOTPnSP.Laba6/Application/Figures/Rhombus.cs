using PluginBase;

namespace Application.Figures;

public class Rhombus(double x, double y, double side, double agile)
    : Figure(x, y, side, side, agile)
{
    public override double Area() => BottomSide * LeftSide * Math.Sin(Angle);
    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}