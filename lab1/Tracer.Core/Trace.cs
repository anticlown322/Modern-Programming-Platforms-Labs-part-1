namespace Tracer.Core;

public class Trace
{
    public string MethodName      { get; }
    public string ClassName       { get; }
    public long   ExecutionTimeMs { get; }
    
    public IReadOnlyList<Trace> NestedMethods { get; }

    public Trace(string methodName, string className, long executionTimeMs, IEnumerable<Trace> nestedMethods)
    {
        MethodName      = methodName;
        ClassName       = className;
        ExecutionTimeMs = executionTimeMs;
        NestedMethods   = new List<Trace>(nestedMethods);
    }
}