namespace OOTPnSP.Laba2.Domain;

public class Triangle : Figure
{
    private double _thirdSide;
    public Triangle(double x, double y, double bottomSide, double leftSide, double angle)
        : base(x, y, bottomSide, leftSide, angle)
    {
        _thirdSide = Math.Sqrt(BottomSide * BottomSide + LeftSide * LeftSide - 2 * BottomSide * LeftSide * Math.Cos(Angle));
    }

    public override double Perimeter() => BottomSide + LeftSide + _thirdSide;

    public override double Area() => 0.5 * BottomSide * LeftSide * Math.Sin(Angle);

    public double InscribedCircleRadius() => 2 * Area() / Perimeter();

    public double CircumscribedCircleRadius() => BottomSide * LeftSide * _thirdSide / (4 * Area());
}