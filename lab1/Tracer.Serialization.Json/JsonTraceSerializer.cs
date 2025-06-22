using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Json;

public class JsonTraceSerializer : ITraceResultSerializer
{
    public string Format => "json";

    public void Serialize(TraceResult traceResult, Stream to)
    {
        using(StreamWriter streamWriter = new StreamWriter(to))
        {
            var json = System.Text.Json.JsonSerializer.Serialize(
                value:   traceResult, 
                options: new System.Text.Json.JsonSerializerOptions
                {
                    IncludeFields = false,
                    MaxDepth  = 1000,
                    WriteIndented = true,
                });
        
            streamWriter.Write(json);
        }
    }
}