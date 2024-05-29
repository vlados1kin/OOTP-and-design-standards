namespace PluginBase;

public interface IZipper
{
    public void CompressAsync(string srcPath, string destPath, SerializationFormat format);
    public void DecompressAsync(string srcPath, SerializationFormat format);
    public string CreateDecompressPath(string srcPath, SerializationFormat format);
}