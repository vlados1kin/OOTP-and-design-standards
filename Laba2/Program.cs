using Laba2.Domain;
using Laba2.Factories;

namespace Laba2;

public class Program
{
    private static List<string> Shapes =
    [
        new ParallelogramFactory(150, 250, 120, 200, Math.PI / 3).Draw(),
        new TriangleFactory(450, 300, 80, 120, Math.PI / 2).Draw(),
        new RectangleFactory(400, 400, 130, 90).Draw()
    ];
    private static Factory GetFactory(Shape shape)
    {
        Factory factory;
        switch (shape.TypeOfFactory)
        {
            case "parallelogram":
                factory = new ParallelogramFactory(shape.X ?? 0, shape.Y ?? 0, shape.BottomSide ?? 0, shape.LeftSide ?? 0, shape.Angle ?? 0);
                break;
            case "rectangle":
                factory = new RectangleFactory(shape.X ?? 0, shape.Y ?? 0, shape.BottomSide ?? 0, shape.LeftSide ?? 0);
                break;
            case "square":
                factory = new SquareFactory(shape.X ?? 0, shape.Y ?? 0, shape.BottomSide ?? 0);
                break;
            case "rhombus":
                factory = new RhombusFactory(shape.X ?? 0, shape.Y ?? 0, shape.BottomSide ?? 0, shape.Angle ?? 0);
                break;
            case "triangle":
                factory = new TriangleFactory(shape.X ?? 0, shape.Y ?? 0, shape.BottomSide ?? 0, shape.LeftSide ?? 0, shape.Angle ?? 0);
                break;
            default:
                throw new ArgumentException("Invalid type");
        }
        return factory;
    }
    
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
            
            if (path == "/editor" && request.Method == "GET")
            {
                await response.WriteAsJsonAsync(Shapes);
            } 
            else if (path == "/editor" && request.Method == "POST")
            {
                try
                {
                    var shape = await request.ReadFromJsonAsync<Shape>();
                    if (shape != null)
                    {
                        Factory factory = GetFactory(shape);
                        Figure figure = factory.Create();
                        var s = factory.Draw(figure);
                        Shapes.Add(s);
                        await response.WriteAsJsonAsync(s);
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

public record class Shape(double? X, double? Y, double? BottomSide, double? LeftSide, double? Angle, string TypeOfFactory);
