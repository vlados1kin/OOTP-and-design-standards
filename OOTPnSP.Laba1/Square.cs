namespace OOTPnSP.Laba1;

internal class Square : Rectangle
{
    public Square(double x, double y, double side) : base(x, y, side, side) { }

    public double InscribedCircleRadius() => _bottomSide / 2;
}