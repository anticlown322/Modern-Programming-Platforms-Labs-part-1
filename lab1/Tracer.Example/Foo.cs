using Tracer.Core;

namespace Tracer.Example;

public class Foo
{
    private Bar _bar;
    private ITracer _tracer;

    internal Foo(ITracer tracer)
    {
        _tracer = tracer;
        _bar = new Bar(_tracer);
    }
    
    public void MyMethod()
    {
        Random random = new Random();
        
        _tracer.StartTrace();
        
        Thread.Sleep(random.Next(100, 200));
        
        _bar.InnerMethod();
        
        Thread.Sleep(random.Next(100, 200));
        
        _tracer.StopTrace();
    }
}
