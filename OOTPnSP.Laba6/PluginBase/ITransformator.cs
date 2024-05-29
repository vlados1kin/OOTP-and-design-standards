namespace PluginBase;

public interface ITransformator
{
    string ToXml(string data);
    string FromXml(string data); 
}