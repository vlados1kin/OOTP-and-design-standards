namespace OOTPnSP.Laba2.Domain;

public class Square : Figure
{
    public Square(double x, double y, double side) : base(x, y, side, side, Math.PI / 2) { }

    public double InscribedCircleRadius() => BottomSide / 2;

    public override double Area() => BottomSide * LeftSide;

    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}