namespace OOTPnSP.Laba2.Domain;

public class Rhombus : Figure
{
    public Rhombus(double x, double y, double side, double agile) : base(x, y, side, side, agile) { }

    public double InscribedCircleRadius() => BottomSide * Math.Sin(Angle) / 2;

    public override double Area() => BottomSide * LeftSide * Math.Sin(Angle);

    public override double Perimeter() => 2 * (BottomSide + LeftSide);
}