using Tracer.Core;

namespace Tracer.Example;

public class Bar
{
    private ITracer _tracer;

    internal Bar(ITracer tracer)
    {
        _tracer = tracer;
    }
    
    public void InnerMethod()
    {
        _tracer.StartTrace();
        
        Thread.Sleep(777);
        
        _tracer.StopTrace();
    }
}
