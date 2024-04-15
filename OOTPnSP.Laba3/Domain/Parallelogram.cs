namespace OOTPnSP.Laba2.Domain;

public class Parallelogram : Figure
{
    public Parallelogram(double x, double y, double bottomSide, double leftSide, double angle) : base(x, y, bottomSide, leftSide, angle) { }

    public override double Area() => BottomSide * LeftSide * Math.Sin(Angle);
        
    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}