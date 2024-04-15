using OOTPnSP.Laba2.Domain;
using OOTPnSP.Laba2.Factories;

namespace OOTPnSP.Laba2;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();
        
        app.Run(async (context) =>
        {
            var response = context.Response;
            var request = context.Request;
            var path = context.Request.Path;
            
            if (path == "/editor" && request.Method == "POST")
            {
                try
                {
                    var shape = await request.ReadFromJsonAsync<Shape>();
                    if (shape != null)
                    {
                        Factory factory = shape.GetFactory();
                        Figure figure = factory.Create();
                        var pattern = factory.Draw(figure);
                        await response.WriteAsJsonAsync(pattern);
                    }
                }
                catch
                {
                    await response.WriteAsJsonAsync(new { Message = "Invalid data" });
                }
            }
        });

        app.Run();
    }
}

public class Shape
{
    public double X { get; init; }
    public double Y { get; init; }
    public double? BottomSide { get; init; }
    public double? LeftSide { get; init; }
    public double? Angle { get; init; }
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
