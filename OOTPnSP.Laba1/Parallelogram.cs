namespace OOTPnSP.Laba1;

internal class Parallelogram : Figure
{
    public Parallelogram(double x, double y, double bottomSide, double leftSide, double angle) : base(x, y, bottomSide, leftSide, angle) { }

    public override double Area() => _bottomSide * _leftSide * Math.Sin(_angle);
        
    public override double Perimeter() => 2 * (_bottomSide + _leftSide);
}