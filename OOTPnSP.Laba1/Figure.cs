using System.Drawing;

namespace OOTPnSP.Laba1;

internal abstract class Figure
{
    protected double _bottomSide;
    protected double _leftSide;
    protected double _angle;
    protected double _x;
    protected double _y;
    
    public Figure(double x, double y, double bottomSide, double leftSide, double angle)
    {
        BottomSide = bottomSide;
        LeftSide = leftSide;
        Angle = angle;
        X = x;
        Y = y;
    }
        
    public double BottomSide
    {
        get => _bottomSide;
        set
        {
            if (value > 0)
                _bottomSide = value;
            else
                throw new ArgumentException("Invalid BottomSide");
        }
    }

    public double LeftSide
    {
        get => _leftSide;
        set
        {
            if (value > 0)
                _leftSide = value;
            else
                throw new ArgumentException("Invalid leftSide");
        }
    }
        
    public double Angle
    {
        get => _angle;
        set
        {
            if (value > 0 && value <= Math.PI / 2)
                _angle = value;
            else
                throw new ArgumentException("Invalid angle");
        }
    }
    
    public double X
    {
        get => _x;
        set
        {
            if (value >= 0)
                _x = value;
            else
                throw new ArgumentException("Invalid X");
        }
    }
    
    public double Y
    {
        get => _y;
        set
        {
            if (value >= 0)
                _y = value;
            else
                throw new ArgumentException("Invalid Y");
        }
    }
    
    public Color Color { get; set; } = Color.White;
        
    public abstract double Area();
    public abstract double Perimeter();
}
