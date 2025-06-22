using Tracer.Core;
using Tracer.Example;
using Tracer.Serialization;

Random             random            = new Random();
Tracer.Core.Tracer tracer            = new Tracer.Core.Tracer();
SerializerManager  serializerManager = new SerializerManager();
Thread[]           threads           = new Thread[random.Next(3,10)];

//init threads
threads[0] = new Thread(() =>
{
    Foo foo = new Foo(tracer);
    foo.MyMethod();
});

threads[1] = new Thread(() =>
{
    C cl = new C(tracer);
    cl.M0();
});

for (int i = 2; i < threads.Length; i++)
{
    threads[i] = new Thread(() =>
    {
        tracer.StartTrace();
        Thread.Sleep(Random.Shared.Next(0, 333));
        tracer.StopTrace();
    });
}

//start threads
foreach (var thread in threads) thread.Start();

//end threads
foreach (var thread in threads) thread.Join();

//serialize results
var traceResult = tracer.GetTraceResult();
serializerManager.LoadSerializers(@"D:\Serializers");
serializerManager.UseSerializer(serializerManager.Serializers[0], traceResult, @"D:\results\");