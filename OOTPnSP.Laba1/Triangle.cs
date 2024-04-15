namespace OOTPnSP.Laba1;

internal class Triangle : Figure
{
    private double _thirdSide;
    public Triangle(double x, double y, double bottomSide, double leftSide, double angle)
        : base(x, y, bottomSide, leftSide, angle)
    {
        _thirdSide = Math.Sqrt(_bottomSide * _bottomSide + _leftSide * _leftSide - 2 * _bottomSide * _leftSide * Math.Cos(_angle));
    }

    public override double Perimeter() => _bottomSide + _leftSide + _thirdSide;

    public override double Area() => 0.5 * _bottomSide * _leftSide * Math.Sin(_angle);

    public double InscribedCircleRadius() => 2 * Area() / Perimeter();

    public double CircumscribedCircleRadius() => _bottomSide * _leftSide * _thirdSide / (4 * Area());
}