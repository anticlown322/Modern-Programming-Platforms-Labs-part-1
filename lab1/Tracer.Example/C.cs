using Tracer.Core;

namespace Tracer.Example;

public class C
{
    private ITracer _tracer;
    private Random  _random;
    
    public C(ITracer tracer)
    {
        _tracer = tracer;
        _random = new Random();
    }

    public void M0()
    {
        M1();
        M2();
    }
    
    private void M1()
    {
        _tracer.StartTrace();
        
        Thread.Sleep(_random.Next(100, 300));
        
        _tracer.StopTrace();
    }
    
    private void M2()
    {
        _tracer.StartTrace();
        
        Thread.Sleep(_random.Next(100, 300));
        
        _tracer.StopTrace();
    }
}