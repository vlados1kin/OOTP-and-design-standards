using System.Runtime.Serialization;
using OOTPnSP.Laba2.Factories;
using OOTPnSP.Laba2.Services;

namespace OOTPnSP.Laba2.Models;

[DataContract]
public class Shape
{
    [DataMember] 
    public int Id = ShapeService.Count++;

    [DataMember]
    public double X { get; init; }
    [DataMember]
    public double Y { get; init; }
    [DataMember]
    public double? BottomSide { get; init; }
    [DataMember]
    public double? LeftSide { get; init; }
    [DataMember]
    public double? Angle { get; init; }
    [DataMember]
    public string? TypeOfFactory { get; init; }

    public Factory GetFactory()
    {
        Factory factory;
        switch (TypeOfFactory)
        {
            case "parallelogram":
                factory = new ParallelogramFactory(X, Y, BottomSide ?? 0, LeftSide ?? 0, Angle ?? 0);
                break;
            case "rectangle":
                factory = new RectangleFactory(X, Y, BottomSide ?? 0, LeftSide ?? 0);
                break;
            case "square":
                factory = new SquareFactory(X, Y, BottomSide ?? 0);
                break;
            case "rhombus":
                factory = new RhombusFactory(X, Y, BottomSide ?? 0, Angle ?? 0);
                break;
            case "triangle":
                factory = new TriangleFactory(X, Y, BottomSide ?? 0, LeftSide ?? 0, Angle ?? 0);
                break;
            default:
                throw new ArgumentException("Invalid type");
        }
        return factory;
    }
}