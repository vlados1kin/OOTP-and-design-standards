using System.Text;
using PluginBase;

namespace Application.Adapter;

public class ConvertorAdapter : IConvertor
{
    private ITransformator? _transformator;
    
    public ConvertorAdapter(ITransformator transformator)
    {
        _transformator = transformator;
    }

    public void FromJsonToXml(string path)
    {
        using FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fsSource.Length];
        int numBytesToRead = (int)fsSource.Length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
            int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
            if (n == 0)
                break;
            numBytesRead += n;
            numBytesToRead -= n;
        }
        
        string xml = _transformator.ToXml(Encoding.Default.GetString(bytes));
        
        string newPath = path.Replace(".json", "_converted.xml");
        FileStream fileStream = new FileStream(newPath, FileMode.OpenOrCreate);
        byte[] byteXml = Encoding.Default.GetBytes(xml);
        fileStream.Write(byteXml, 0, byteXml.Length);
        fileStream.Close();
    }

    public void FromXmlToJson(string path)
    {
        using FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fsSource.Length];
        int numBytesToRead = (int)fsSource.Length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
            int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
            if (n == 0)
                break;
            numBytesRead += n;
            numBytesToRead -= n;
        }
        
        string json = _transformator.FromXml(Encoding.Default.GetString(bytes));
        
        string newPath = path.Replace(".xml", "_converted.json");
        FileStream fileStream = new FileStream(newPath, FileMode.OpenOrCreate);
        byte[] byteXml = Encoding.Default.GetBytes(json);
        fileStream.Write(byteXml, 0, byteXml.Length);
        fileStream.Close();
    }
}