namespace PluginBase;

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
    protected Factory() {}

    protected double X { get; init; }
    protected double Y { get; init; }
    protected double BottomSide { get; init; }
    protected double LeftSide { get; init; }
    protected double Angle { get; init; }

    public abstract string TypeOfFactory();
    public abstract string NameOfFactory();
    public abstract Figure Create();
    public abstract string Draw(Figure figure);
}