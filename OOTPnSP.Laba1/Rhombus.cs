namespace OOTPnSP.Laba1;

internal class Rhombus : Parallelogram
{
    public Rhombus(double x, double y, double side, double agile) : base(x, y, side, side, agile) { }

    public double InscribedCircleRadius() => _bottomSide * Math.Sin(_angle) / 2;
}