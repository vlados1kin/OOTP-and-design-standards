using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Application.Data;
using Application.Services;
using PluginBase;

namespace Application.Adapter;

public class Convertor : IConvertor
{
    public void FromJsonToXml(string path)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Shape>));
        FileStream fileStream = new FileStream(path, FileMode.Open);
        List<Shape>? shapes = serializer.ReadObject(fileStream) as List<Shape>;
        fileStream.Close();
        string newPath = path.Replace(".json", "_converted.xml");
        DataContractSerializer dcSerializer = new DataContractSerializer(typeof(List<Shape>));
        fileStream = new FileStream(newPath, FileMode.OpenOrCreate);
        dcSerializer.WriteObject(fileStream, shapes);
        fileStream.Close();
    }

    public void FromXmlToJson(string path)
    {
        DataContractSerializer dcSerializer = new DataContractSerializer(typeof(List<Shape>));
        FileStream fileStream = new FileStream(path, FileMode.Open);
        List<Shape>? shapes = dcSerializer.ReadObject(fileStream) as List<Shape>;
        fileStream.Close();
        string newPath = path.Replace(".xml", "_converted.json");
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Shape>));
        fileStream = new FileStream(newPath, FileMode.OpenOrCreate);
        serializer.WriteObject(fileStream, shapes);
        fileStream.Close();
    }
}