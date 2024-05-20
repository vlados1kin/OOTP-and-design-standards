using System.Reflection;
using System.Runtime.Serialization;
using Application.Factories;
using Application.PluginManager;
using Application.Services;
using PluginBase;

namespace Application.Data;

[DataContract]
public class Shape
{
    [DataMember] 
    public int Id = FigureService.Count++;

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
        Factory? factory = null;
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
                if (FigureService.Dll.ContainsKey(TypeOfFactory!))
                {
                    Assembly plugin = FigureService.Dll[TypeOfFactory!];
                    factory = PluginService.GetFactory(plugin, X, Y, BottomSide ?? 0, LeftSide ?? 0, Angle ?? 0);
                }
                break;
        }
        return factory ?? throw new ArgumentException("This type of Factory does not exist");
    }
}