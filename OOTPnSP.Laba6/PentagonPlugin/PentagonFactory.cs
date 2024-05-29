using System;
using PluginBase;

namespace PentagonPlugin;

public class PentagonFactory : Factory
{
    public PentagonFactory(double x, double y, double bottomSide, double leftSide, double angle) : base(x, y, bottomSide, bottomSide, Math.PI * 3 / 5) { }
    public override string TypeOfFactory() => "pentagon";
    public override string NameOfFactory() => "Пентагон";
    public override Figure Create() => new Pentagon(X, Y, BottomSide, LeftSide, Angle);

    public override string Draw(Figure figure)
    {
        var x1 = Math.Round(X);
        var y1 = Math.Round(Y);
        
        var x2 = Math.Round(x1 - figure.BottomSide * Math.Cos(2 * Math.PI / 5));
        var y2 = Math.Round(y1 - figure.BottomSide * Math.Sin(2 * Math.PI / 5));
        
        var x3 = Math.Round(x2 + figure.BottomSide * Math.Cos(Math.PI / 5));
        var y3 = Math.Round(y2 - figure.BottomSide * Math.Sin(Math.PI / 5));
        
        var x4 = Math.Round(x3 + figure.BottomSide * Math.Cos(Math.PI / 5));
        var y4 = Math.Round(y2);
        
        var x5 = Math.Round(x4 - figure.BottomSide * Math.Cos(2 * Math.PI / 5));
        var y5 = Math.Round(y1);

        return $"M{x1} {y1} L{x2} {y2} L{x3} {y3} L{x4} {y4} L{x5} {y5} L{x1} {y1}";
    }
    
    public override Factory Clone()
    {
        return new PentagonFactory(X, Y, BottomSide, LeftSide, Angle);
    }
}