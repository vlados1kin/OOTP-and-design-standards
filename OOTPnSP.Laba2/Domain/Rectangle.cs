namespace OOTPnSP.Laba2.Domain;

public class Rectangle : Figure
{
    public Rectangle(double x, double y, double width, double height) : base(x, y, width, height, Math.PI / 2) { }

    public double CircumscribedCircleRadius() => Math.Sqrt(BottomSide * BottomSide + LeftSide * LeftSide) / 2;

    public override double Area() => BottomSide * LeftSide;

    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}