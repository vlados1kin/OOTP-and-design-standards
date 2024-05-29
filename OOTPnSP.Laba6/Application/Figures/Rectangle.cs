using PluginBase;

namespace Application.Figures;

public class Rectangle(double x, double y, double width, double height)
    : Figure(x, y, width, height, Math.PI / 2)
{
    public override double Area() => BottomSide * LeftSide;
    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}