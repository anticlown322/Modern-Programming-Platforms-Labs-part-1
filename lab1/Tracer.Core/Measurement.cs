using System.Diagnostics;

namespace Tracer.Core;

public class Measurement
{
    private readonly string    _className;
    private readonly string    _methodName;
    private readonly Stopwatch _stopwatch;
        
    private readonly List<Measurement> _nestedMethods = new();

    public Measurement(string className, string methodName)
    {
        _className  = className;
        _methodName = methodName;
        _stopwatch  = Stopwatch.StartNew();
    }

    public void Stop() => _stopwatch.Stop();
        
    public void AddNestedMethod(Measurement measurement) => _nestedMethods.Add(measurement);

    public Trace ToMethodTrace()
    {
        return new Trace(
            methodName:      _methodName, 
            className:       _className, 
            executionTimeMs: _stopwatch.ElapsedMilliseconds, 
            nestedMethods:   _nestedMethods.Select(m => m.ToMethodTrace()));
    }
}