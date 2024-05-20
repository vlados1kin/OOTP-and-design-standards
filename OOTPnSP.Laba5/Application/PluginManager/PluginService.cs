using System.Reflection;
using PluginBase;

namespace Application.PluginManager;

public static class PluginService
{
    public static Assembly LoadPlugin(string path)
    {
        PluginLoadContext loadContext = new PluginLoadContext(path);
        return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
    }

    public static Factory? GetFactory(Assembly assembly, params double[] args)
    {
        Type? pluginType = assembly.GetTypes().FirstOrDefault(type => typeof(Factory).IsAssignableFrom(type), null);
        return pluginType != null
            ? Activator.CreateInstance(pluginType, args[0], args[1], args[2], args[3], args[4]) as Factory
            : null;
    }

    public static Signature? GetSignature(Assembly assembly)
    {
        Type? signatureType = assembly.GetTypes().FirstOrDefault(type => typeof(Signature).IsAssignableFrom(type), null);
        return signatureType != null
            ? Activator.CreateInstance(signatureType) as Signature
            : null;
    }
    
    public static IEncryptor? GetEncryptor(Assembly assembly)
    {
        Type? encryptorType = assembly.GetTypes().FirstOrDefault(type => typeof(IEncryptor).IsAssignableFrom(type), null);
        return encryptorType != null 
            ? Activator.CreateInstance(encryptorType) as IEncryptor
            : null;
    }

    public static IZipper? GetZipper(Assembly assembly)
    {
        Type? encryptorType = assembly.GetTypes().FirstOrDefault(type => typeof(IZipper).IsAssignableFrom(type), null);
        return encryptorType != null 
            ? Activator.CreateInstance(encryptorType) as IZipper
            : null;
    }
}