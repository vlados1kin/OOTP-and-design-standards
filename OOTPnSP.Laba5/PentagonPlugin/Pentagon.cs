using System;
using PluginBase;

namespace PentagonPlugin;

public class Pentagon(double x, double y, double bottomSide, double leftSide, double angle) : Figure(x, y, bottomSide, bottomSide, Math.PI * 3 / 5)
{
    public override double Area() => 5 * BottomSide * BottomSide * Math.Tan(Math.PI * 3 / 10) / 4;
    public override double Perimeter() => 5 * BottomSide;
}