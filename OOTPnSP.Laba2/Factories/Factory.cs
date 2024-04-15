using OOTPnSP.Laba2.Domain;

namespace OOTPnSP.Laba2.Factories;

public abstract class Factory
{
    protected Factory(double x, double y, double bottomSide, double leftSide, double angle)
    {
        X = x;
        Y = y;
        BottomSide = bottomSide;
        LeftSide = leftSide;
        Angle = angle;
    }

    protected double X { get; init; }
    protected double Y { get; init; }
    protected double BottomSide { get; init; }
    protected double LeftSide { get; init; }
    protected double Angle { get; init; }

    public abstract Figure Create();
    

    public abstract string Draw(Figure figure);
}