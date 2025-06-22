using Tracer.Core;

namespace Trace.Core.Tests;

public class ThreadTraceTests
{
    [Theory]
    [InlineData(100, 300, 400)]
    [InlineData(1000, 1000, 2000)]
    public void ThreadTrace_Constructor_Test(int firstThreadTime, int secondThreadTime, long minExpectedTime)
    {
        #region Arrange

        var traces = new List<Tracer.Core.Trace>();
        
        //first measurement
        var measurement  = new Measurement("foo", "testClass");
        Thread.Sleep(firstThreadTime);
        measurement.Stop();
        traces.Add(measurement.ToMethodTrace());
        
        //second measurement
        measurement = new Measurement("bar", "testClass");
        Thread.Sleep(secondThreadTime);
        measurement.Stop();
        traces.Add(measurement.ToMethodTrace());
        
        #endregion Arrange
        
        #region Act
        
        var threadTrace = new ThreadTrace(1, traces);
        
        #endregion Act
        
        #region Assert
        
        Assert.NotNull(threadTrace);
        Assert.IsType<ThreadTrace>(threadTrace);
        Assert.Equal(1, threadTrace.ThreadId);
        Assert.Equal(2, threadTrace.Traces.Count);
        Assert.True(threadTrace.TotalExecutionTimeMs >= minExpectedTime);
        
        #endregion Assert
    }
}