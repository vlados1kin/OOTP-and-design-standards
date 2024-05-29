namespace PluginBase;

public interface IConvertor
{
    public void FromJsonToXml(string path);
    public void FromXmlToJson(string path);
}