using Tracer.Core;

namespace Trace.Core.Tests;

public class MeasurementTests
{
    [Fact]
    public void Measurement_AddNestedMethod_ReturnVoid()
    {
        #region Arrange

        //root measurement
        Measurement measurement = new Measurement("foo", "bar");
        Thread.Sleep(100);
        measurement.Stop();
        
        //nested measurement
        Measurement nestedMeasurement = new Measurement("damn", "bar");
        Thread.Sleep(300);
        measurement.Stop();
        
        #endregion Arrange
        
        #region Act
        
        measurement.AddNestedMethod(nestedMeasurement);
        
        #endregion Act
        
        #region Assert
        
        var trace = measurement.ToMethodTrace();
        Assert.Single(trace.NestedMethods);
        Assert.Equal("bar", trace.NestedMethods[0].MethodName);
        Assert.Equal("damn", trace.NestedMethods[0].ClassName);
        Assert.True(trace.NestedMethods[0].ExecutionTimeMs >= 300);

        #endregion Assert
    }
    
    [Fact]
    public void Measurement_ToMethodTrace_ReturnTrace()
    {
        #region Arrange

        //root measurement
        Measurement measurement = new Measurement("foo", "bar");
        Thread.Sleep(100);
        measurement.Stop();
        
        //nested measurement
        Measurement nestedMeasurement = new Measurement("damn", "bar");
        Thread.Sleep(300);
        measurement.Stop();
        
        measurement.AddNestedMethod(nestedMeasurement);
        
        #endregion Arrange

        #region Act

        var result = measurement.ToMethodTrace();

        #endregion Act

        #region Assert

        Assert.NotNull(result);
        Assert.IsType<Tracer.Core.Trace>(result);
        Assert.NotEmpty(result.NestedMethods);
        Assert.Equal("foo", result.ClassName);
        Assert.Equal("bar", result.MethodName);
        Assert.True(result.ExecutionTimeMs >= 100);
        Assert.Single(result.NestedMethods);

        #endregion Assert
    }
}