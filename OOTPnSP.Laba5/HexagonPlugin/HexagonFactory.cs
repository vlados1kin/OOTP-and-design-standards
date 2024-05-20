using System;
using PluginBase;

namespace HexagonPlugin;

public class HexagonFactory : Factory
{
    public HexagonFactory(double x, double y, double bottomSide, double leftSide, double angle) : base(x, y, bottomSide, bottomSide, Math.PI * 2 / 3) { }
    public override string TypeOfFactory() => "hexagon";
    public override string NameOfFactory() => "Гексагон";
    public override Figure Create() => new Hexagon(X, Y, BottomSide, LeftSide, Angle);
    public override string Draw(Figure figure)
    {
        var x1 = Math.Round(X);
        var y1 = Math.Round(Y);

        var x2 = Math.Round(x1 - 0.5 * BottomSide);
        var y2 = Math.Round(y1 - Math.Cos(Math.PI / 6) * BottomSide);

        var x3 = x1;
        var y3 = Math.Round(y2 - Math.Cos(Math.PI / 6) * BottomSide);

        var x4 = x3 + BottomSide;
        var y4 = y3;

        var x5 = Math.Round(x4 + 0.5 * BottomSide);
        var y5 = y2;

        var x6 = x4;
        var y6 = y1;

        return $"M{x1} {y1} L{x2} {y2} L{x3} {y3} L{x4} {y4} L{x5} {y5} L{x6} {y6} L{x1} {y1}";
    }
}