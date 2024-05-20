using PluginBase;

namespace Application.Figures;

public class Square(double x, double y, double side) : Figure(x, y, side, side, Math.PI / 2)
{
    public override double Area() => BottomSide * LeftSide;
    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}