using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace Tracer.Core;

public class Tracer : ITracer
{
    private readonly ConcurrentDictionary<int, Stack<Measurement>> _methodStacks       = new();
    private readonly ConcurrentDictionary<int, List<Measurement>>  _threadMeasurements = new();

    public void StartTrace()
    {
        StackTrace  stackTrace = new StackTrace(1);
        StackFrame? frame      = stackTrace.GetFrame(0);
        MethodBase? method     = frame?.GetMethod();
        
        string className  = method?.DeclaringType?.Name ?? "UnknownClass";
        string methodName = method?.Name ?? "UnknownMethod";

        Measurement measurement = new Measurement(className, methodName);
        int         threadId    = Thread.CurrentThread.ManagedThreadId;

        _methodStacks.GetOrAdd(
            key:          threadId, 
            valueFactory: _ => new Stack<Measurement>()
            ).Push(measurement);
    }

    public void StopTrace()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;

        if (_methodStacks.TryGetValue(threadId, out var stack) && stack.Any())
        {
            Measurement measurement = stack.Pop();
            measurement.Stop();
            
            //if stack.Count > 0 then it is a nested method. else it is root method
            if (stack.Any())
            {
                stack.Peek().AddNestedMethod(measurement);
            }
            else
            {
                _threadMeasurements.GetOrAdd(
                    key:          threadId, 
                    valueFactory: _ => new List<Measurement>())
                    .Add(measurement);
            }
        }
    }

    public TraceResult GetTraceResult()
    {
        var threadTraces = _threadMeasurements
            .Select(pair => new ThreadTrace(
                threadId: pair.Key, 
                traces:   pair.Value.Select(m => m.ToMethodTrace()))
            );

        return new TraceResult(threadTraces);
    }
}