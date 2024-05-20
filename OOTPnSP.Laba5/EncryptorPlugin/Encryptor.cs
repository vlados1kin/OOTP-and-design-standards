using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PluginBase;

namespace EncryptorPlugin;

public class Encryptor : IEncryptor
{
    public void EncryptString(string path, string key)
    {
        string src;
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            StringBuilder builder = new StringBuilder(256);
            byte[] buffer = new byte[64];
            int i;
            while ((i = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string data = Encoding.UTF8.GetString(buffer, 0, i);
                builder.Append(data);
            }
            src = builder.ToString();
        }
        
        try
        {
            string dir = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            using (FileStream fileStream = new(Path.Combine(dir, name + "_encrypted.data"), FileMode.OpenOrCreate))
            {
                using Aes aes = Aes.Create();
                byte[] inKey = Encoding.UTF8.GetBytes(key);
                aes.Key = inKey;
                    
                byte[] iv = aes.IV;
                fileStream.Write(iv, 0, iv.Length);

                using CryptoStream cryptoStream = new(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                using StreamWriter encryptWriter = new(cryptoStream);
                encryptWriter.WriteLine(src);
            }
            Console.WriteLine("The file was encrypted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The encryption failed. {ex.Message}");
        }
    }

    public async Task DecryptString(string path, string key, SerializationFormat format)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Open);
            using Aes aes = Aes.Create();
            byte[] iv = new byte[aes.IV.Length];
            int numBytesToRead = aes.IV.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }

            byte[] inKey = Encoding.UTF8.GetBytes(key);

            await using CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(inKey, iv), CryptoStreamMode.Read);
            using StreamReader decryptReader = new(cryptoStream);
            string decryptedMessage = await decryptReader.ReadToEndAsync();
            path = path.Replace("_encrypted.data", format == SerializationFormat.Xml ? "_decrypted.xml" : "_decrypted.json");
            await using FileStream file = new(path, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buffer = Encoding.UTF8.GetBytes(decryptedMessage);
            await file.WriteAsync(buffer, 0, buffer.Length);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The decryption failed. {ex}");
        }
    }

    public string CreateDecryptedPath(string srcPath, SerializationFormat format)
    {
        string extension = format == SerializationFormat.Xml ? "xml" : "json";
        string newPath = srcPath.Replace("_encrypted.data", "_decrypted." + extension);
        return newPath;
    }
}