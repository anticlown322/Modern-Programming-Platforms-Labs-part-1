using Tracer.Core;

namespace Trace.Core.Tests;

public class TracerTests
{
    [Fact]
    public void Tracer_StartTrace_ReturnVoid()
    {
        #region Arrange

        var tracer = new Tracer.Core.Tracer();
        
        #endregion Arrange
        
        #region Act
        
        tracer.StartTrace();
        
        #endregion Act
        
        #region Assert

        TraceResult result = tracer.GetTraceResult();
        Assert.Empty(result.Threads);
        tracer.StopTrace();

        #endregion Assert
    }
    
    [Fact]
    public void Tracer_StopTrace_ReturnVoid()
    {
        #region Arrange

        var tracer = new Tracer.Core.Tracer();
            
        tracer.StartTrace();
        Thread.Sleep(100);
        
        #endregion Arrange
        
        #region Act
        
        tracer.StopTrace();
        
        #endregion Act
        
        #region Assert
        
        TraceResult result = tracer.GetTraceResult();
        Assert.Single(result.Threads);
        Assert.Single(result.Threads[0].Traces);
        Assert.True(result.Threads[0].Traces[0].ExecutionTimeMs >= 100);
        
        #endregion Assert
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(5)]
    public void Tracer_GetTraceResult_ReturnTraceResult_SignleThread(int nestingLevel)
    {
        #region Arrange

        var tracer = new Tracer.Core.Tracer();

        for (int i = 0; i < nestingLevel; i++)
        {
            tracer.StartTrace();
            Thread.Sleep(50);
        }
        for (int i = 0; i < nestingLevel; i++)
        {
            tracer.StopTrace();
        }
        
        #endregion Arrange
        
        #region Act
        
        TraceResult result = tracer.GetTraceResult();
        
        #endregion Act
        
        #region Assert
        
        Tracer.Core.Trace rootTrace = result.Threads[0].Traces[0];

        void NestingLevelCheck(Tracer.Core.Trace currTrace, int currNestingLevel)
        {
            if (currNestingLevel == nestingLevel)
                return;

            currNestingLevel++;
            Assert.Single(currTrace.NestedMethods);
            Assert.True(currTrace.ExecutionTimeMs >= 50);
            NestingLevelCheck(currTrace.NestedMethods[0], currNestingLevel);
        }

        NestingLevelCheck(rootTrace, 1);

        #endregion Assert
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void Tracer_GetTraceResult_ReturnTraceResult_ManyThreads(int threadCount)
    {
        #region Arrange

        var tracer = new Tracer.Core.Tracer();
        var threads = new Thread[threadCount];
        Random rnd = new Random();

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                tracer.StartTrace();
                Thread.Sleep(rnd.Next(100,200));
                tracer.StopTrace();
            });
            
            threads[i].Start();
        }
        
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        
        #endregion Arrange
        
        #region Act
        
        TraceResult result = tracer.GetTraceResult();
        
        #endregion Act
        
        #region Assert
        
        Assert.Equal(threadCount, result.Threads.Count);

        foreach (ThreadTrace threadTrace in result.Threads)
        {
            Assert.True(threadTrace.Traces[0].ExecutionTimeMs >= 100);
        }
        
        #endregion Assert
    }
}