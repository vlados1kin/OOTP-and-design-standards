using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Application.Data;
using PluginBase;

namespace Application.Services;

public static class FigureService
{
    private static List<Shape> _shapes = new();
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
    
    public static IEncryptor? EncryptPlugin = null;
    public static string? EncryptionKey { get; set; }
    public static IZipper? ZipperPlugin = null;

    public static ITransformator? TransformatorPlugin { get; set; }

    public static string FilePath { get; set; } =
        FigureService.Format == SerializationFormat.Json
            ? @"d:\BSUIR\course-2\OOTPnSP\OOTPnSP.Laba6\Application\UploadedFigures\undefined.json"
            : @"d:\BSUIR\course-2\OOTPnSP\OOTPnSP.Laba6\Application\UploadedFigures\undefined.xml";

    public static List<Shape>? GetAll() => _shapes;

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
    
    public static void InitList(string path)
    {
        switch (Format)
        {
            case SerializationFormat.Json:
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Shape>));
                FileStream fileStream = new FileStream(path, FileMode.Open);
                List<Shape>? shapes = serializer.ReadObject(fileStream) as List<Shape>;
                _shapes = shapes!;
                Count = _shapes.Count;
                fileStream.Close();
                break;
            case SerializationFormat.Xml:
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
                string jsonPath = FilePath;
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Shape>));
                FileStream fileStream = new FileStream(jsonPath, FileMode.Truncate);
                serializer.WriteObject(fileStream, _shapes);
                fileStream.Close();
                break;
            case SerializationFormat.Xml:
                jsonPath = FilePath;
                DataContractSerializer dcSerializer = new DataContractSerializer(typeof(List<Shape>));
                fileStream = new FileStream(jsonPath, FileMode.Truncate);
                dcSerializer.WriteObject(fileStream, _shapes);
                fileStream.Close();
                break;
        }
    }
}