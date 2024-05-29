using System.Security.Cryptography;
using System.Text;

namespace PluginBase;

public interface IEncryptor
{
    public void EncryptString(string path, string key);

    public Task DecryptString(string path, string key, SerializationFormat format);
    public string CreateDecryptedPath(string srcPath, SerializationFormat format);
}