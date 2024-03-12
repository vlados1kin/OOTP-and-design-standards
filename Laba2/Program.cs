using Laba2.Domain;
using Laba2.Factories;

namespace Laba2;

public class Program
{
    private static Shapes _shapes = new Shapes(new List<Shape>(16)
    {
        new Shape(new Parallelogram(150, 250, 120, 200, Math.PI / 3)) { TypeOfFactory = "parallelogram"},
        new Shape(new Triangle(450, 300, 80, 120, Math.PI / 2)) { TypeOfFactory = "triangle"},
        new Shape(new Rectangle(400, 400, 130, 90)) { TypeOfFactory = "rectangle"},
        new Shape(new Square(600, 400, 150)) { TypeOfFactory = "square"},
    });
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
                List<string> patterns = new List<string>(16);
                for (int i = 0; i < _shapes.Length; i++)
                {
                    Shape shape = _shapes[i];
                    Factory factory = shape.GetFactory();
                    Figure figure = factory.Create();
                    patterns.Add(factory.Draw(figure));
                }
                await response.WriteAsJsonAsync(patterns);
            } 
            else if (path == "/editor" && request.Method == "POST")
            {
                try
                {
                    var shape = await request.ReadFromJsonAsync<Shape>();
                    if (shape != null)
                    {
                        Factory factory = shape.GetFactory();
                        Figure figure = factory.Create();
                        var pattern = factory.Draw(figure);
                        _shapes.Add(shape);
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
    public required string? TypeOfFactory { get; init; }
    public Shape() { }

    public Shape(Figure figure)
    {
        X = figure.X;
        Y = figure.Y;
        BottomSide = figure.BottomSide;
        LeftSide = figure.LeftSide;
        Angle = figure.Angle;
    }

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

public class Shapes
{
    private List<Shape> _shapes;
    public Shapes(List<Shape> shapes) => _shapes = shapes;
    public void Add(Shape shape) => _shapes.Add(shape);
    public int Length => _shapes.Count;
    public Shape this[int index]
    {
        get => index >= 0 && index < _shapes.Count
            ? _shapes[index]
            : throw new IndexOutOfRangeException("Invalid index");
        set
        {
            if (index >= 0 && index < _shapes.Count)
                _shapes[index] = value;
            else
                throw new IndexOutOfRangeException("Invalid index");
        }
    }
}
