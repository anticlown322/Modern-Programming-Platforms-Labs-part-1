namespace Tracer.Core;

public class TraceResult
{
    public IReadOnlyList<ThreadTrace> Threads { get; }

    public TraceResult(IEnumerable<ThreadTrace> threadTraces)
    {
        Threads = new List<ThreadTrace>(threadTraces);
    }
}