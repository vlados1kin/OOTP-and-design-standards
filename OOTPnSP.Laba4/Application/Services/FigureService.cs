using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Application.Data;

namespace Application.Services;

public static class FigureService
{
    static FigureService()
    {
        InitList(SerializationFormat.Xml);
    }
    
    private static List<Shape> _shapes;
    public static int Count;
    public static SerializationFormat Format;
    public static Dictionary<string, Assembly> Dll = new();
    public static Dictionary<string, string> Types = new()
    {
        {"parallelogram", "Параллелограмм"},
        {"rectangle", "Прямоугольник"},
        {"rhombus", "Ромб"},
        {"square", "Квадрат"},
        {"triangle", "Треугольник"}
    };
    
    public static List<Shape> GetAll() => _shapes;

    public static Shape? Get(int id) => _shapes.FirstOrDefault(s => s.Id == id);

    public static void Add(Shape shape) => _shapes.Add(shape);

    public static void Update(Shape shape)
    {
        int index = _shapes.FindIndex(s => s.Id == shape.Id);
        if (index == -1)
            return;
        _shapes[index] = shape;
    }

    public static void Remove(int id)
    {
        Shape? shape = Get(id);
        if (shape == null)
            return;
        _shapes.Remove(shape);
    }

    public static void RemoveAll()
    {
        _shapes.RemoveAll(_ => true);
    }

    private static void InitList(SerializationFormat format)
    {
        switch (format)
        {
            case SerializationFormat.Json:
                string path = $"{Directory.GetCurrentDirectory()}/shapes.json";
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Shape>));
                FileStream fileStream = new FileStream(path, FileMode.Open);
                List<Shape>? shapes = serializer.ReadObject(fileStream) as List<Shape>;
                _shapes = shapes!;
                Count = _shapes.Count;
                fileStream.Close();
                break;
            case SerializationFormat.Xml:
                path = $"{Directory.GetCurrentDirectory()}/shapes.xml";
                DataContractSerializer dcSerializer = new DataContractSerializer(typeof(List<Shape>));
                fileStream = new FileStream(path, FileMode.Open);
                shapes = dcSerializer.ReadObject(fileStream) as List<Shape>;
                _shapes = shapes!;
                Count = _shapes.Count;
                fileStream.Close();
                break;
        }
    }

    public static void SaveChanges()
    {
        switch (Format)
        {
            case SerializationFormat.Json:
                string jsonPath = $"{Directory.GetCurrentDirectory()}/shapes.json";
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Shape>));
                FileStream fileStream = new FileStream(jsonPath, FileMode.Truncate);
                serializer.WriteObject(fileStream, _shapes);
                fileStream.Close();
                break;
            case SerializationFormat.Xml:
                jsonPath = $"{Directory.GetCurrentDirectory()}/shapes.xml";
                DataContractSerializer dcSerializer = new DataContractSerializer(typeof(List<Shape>));
                fileStream = new FileStream(jsonPath, FileMode.Truncate);
                dcSerializer.WriteObject(fileStream, _shapes);
                fileStream.Close();
                break;
        }
    }
}