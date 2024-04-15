using System.Drawing;

namespace OOTPnSP.Laba2.Domain;

public abstract class Figure
{
    protected Figure(double x, double y, double bottomSide, double leftSide, double angle)
    {
        X = x;
        Y = y;
        BottomSide = bottomSide;
        LeftSide = leftSide;
        Angle = angle;
    }
    public double X { get; init; }
    public double Y { get; init; }
    public double BottomSide { get; init; }
    public double LeftSide { get; init; }
    public double Angle { get; init; }

    public abstract double Area();
    public abstract double Perimeter();
}