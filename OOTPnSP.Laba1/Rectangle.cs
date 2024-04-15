namespace OOTPnSP.Laba1;

internal class Rectangle : Parallelogram
{
    public Rectangle(double x, double y, double width, double height) : base(x, y, width, height, Math.PI / 2) { }

    public double CircumscribedCircleRadius() => Math.Sqrt(_bottomSide * _bottomSide + _leftSide * _leftSide) / 2;
}