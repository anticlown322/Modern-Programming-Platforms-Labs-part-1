using System.Reflection;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization;

public class SerializerManager
{
    public IList<ITraceResultSerializer> Serializers { get; } = new List<ITraceResultSerializer>();

    public void LoadSerializers(string directory = "")
    {
        string serializersDirectory = directory.Equals("") 
            ? Path.Combine(Directory.GetCurrentDirectory(), "Serializers") 
            : directory;

        if (!Directory.Exists(serializersDirectory))
        {
            Console.WriteLine("Serializers directory not found.");
            return;
        }
        
        string[] assemblies = Directory.GetFiles(serializersDirectory, "*.dll");
        foreach (string assemblyPath in assemblies)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                
                var types = assembly.GetTypes()
                    .Where(t => typeof(ITraceResultSerializer).IsAssignableFrom(t) && t is
                    {
                        IsInterface: false
                    });

                foreach (Type type in types)
                {
                    if (Activator.CreateInstance(type) is ITraceResultSerializer serializer)
                    {
                        Serializers.Add(serializer);
                    }
                }
            }
            catch
            {
                throw new Exception("Can't load serializers from assembly.");
            }
        }
    }

    public void UseSerializer(ITraceResultSerializer serializer, TraceResult traceResult, string path = "")
    {
        string fileName = Path.Combine(path, $"result.{serializer.Format}");

        using (FileStream fileStream = new FileStream(
                   path: fileName, 
                   mode: FileMode.Create))
        {
            serializer.Serialize(traceResult, fileStream);
        }
    }
}