using System.Collections;
using Faker.Core.Shared;
using Faker.Core.SpecialGenerators;

namespace Faker.Tests;

public class SpecialTypesGenTests
{
    private readonly IFaker _systemUnderTest = new Core.Faker();

    #region DateTime

    [Fact]
    public void DateTimeGenerator_Generate_ReturnDateTime()
    {
        var randomDateTimeVariable = _systemUnderTest.Create<DateTime>();

        Assert.IsType<DateTime>(randomDateTimeVariable);
    }

    [Fact]
    public void DateTimeGenerator_CanGenerate_ReturnTrue()
    {
        DateTimeGenerator gen = new();

        bool result = gen.CanGenerate(typeof(DateTime));

        Assert.True(result);
    }

    [Fact]
    public void DateTimeGenerator_CanGenerate_ReturnFalse()
    {
        DateTimeGenerator gen = new();

        bool result = gen.CanGenerate(typeof(int));

        Assert.False(result);
    }

    #endregion

    #region Enumerable

    [Fact]
    public void EnumerableGenerator_Generate_ReturnNotEmptyEnumerable()
    {
        var randomEnumerableValue = _systemUnderTest.Create<IEnumerable<int>>();

        Assert.IsAssignableFrom<IEnumerable>(randomEnumerableValue);
        Assert.NotNull(randomEnumerableValue);
        Assert.NotEmpty(randomEnumerableValue);
    }

    [Fact]
    public void EnumerableGenerator_CanGenerate_ReturnTrue()
    {
        EnumerableGenerator gen = new();

        bool result = gen.CanGenerate(typeof(IEnumerable<int>));

        Assert.True(result);
    }

    [Fact]
    public void EnumerableGenerator_CanGenerate_ReturnFalse()
    {
        EnumerableGenerator gen = new();

        bool result = gen.CanGenerate(typeof(int));

        Assert.False(result);
    }

    #endregion
}