using System.IO;
using System.IO.Compression;
using PluginBase;

namespace ZipperPlugin;

public class Zipper : IZipper
{
    public void CompressAsync(string srcPath, string destPath, SerializationFormat format)
    {
        using FileStream srcStream = new FileStream(srcPath, FileMode.Open);
        using FileStream destStream = new FileStream(destPath, FileMode.OpenOrCreate);
        using GZipStream stream = new GZipStream(destStream, CompressionMode.Compress);
        srcStream.CopyTo(stream);
    }

    public void DecompressAsync(string srcPath, SerializationFormat format)
    {
        using FileStream srcStream = new FileStream(srcPath, FileMode.Open);
        using FileStream destStream = new FileStream(CreateDecompressPath(srcPath, format), FileMode.OpenOrCreate);
        using GZipStream stream = new GZipStream(srcStream, CompressionMode.Decompress);
        stream.CopyTo(destStream);
    }

    public string CreateDecompressPath(string srcPath, SerializationFormat format)
    {
        string extension = format == SerializationFormat.Xml ? "xml" : "json";
        string newPath = srcPath.Replace(".gz", "_decompressed." + extension);
        return newPath;
    } 
}