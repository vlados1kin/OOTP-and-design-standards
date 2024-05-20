using System;
using PluginBase;

namespace HexagonPlugin;

public class Hexagon(double x, double y, double bottomSide, double leftSide, double angle) : Figure(x, y, bottomSide, bottomSide, Math.PI * 2 / 3)
{
    public override double Area() => 3 * Math.Sqrt(3) * Math.Pow(BottomSide, 2) / 2;
    public override double Perimeter() => 6 * BottomSide;
}