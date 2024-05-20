using System.Reflection;
using Application.Factories;
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
        Type pluginType = assembly.GetTypes().First(type => typeof(Factory).IsAssignableFrom(type));
        return Activator.CreateInstance(pluginType, args[0], args[1], args[2], args[3], args[4]) as Factory;
    }

    public static Signature? GetSignature(Assembly assembly)
    {
        Type signatureType = assembly.GetTypes().First(type => typeof(Signature).IsAssignableFrom(type));
        return Activator.CreateInstance(signatureType) as Signature;
    }
}