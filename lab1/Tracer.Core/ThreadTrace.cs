namespace Tracer.Core;

public class ThreadTrace
{
    public int  ThreadId             { get; }
    public long TotalExecutionTimeMs { get; }
    
    public IReadOnlyList<Trace> Traces { get; }

    public ThreadTrace(int threadId, IEnumerable<Trace> traces)
    {
        ThreadId = threadId;
        Traces   = new List<Trace>(traces);
        
        TotalExecutionTimeMs = Traces.Sum(m => m.ExecutionTimeMs);
    }
}