namespace PluginBase;

public abstract class Figure(double x, double y, double bottomSide, double leftSide, double angle)
{
    public double X { get; init; } = x;
    public double Y { get; init; } = y;
    public double BottomSide { get; init; } = bottomSide;
    public double LeftSide { get; init; } = leftSide;
    public double Angle { get; init; } = angle;

    public abstract double Area();
    public abstract double Perimeter();
}