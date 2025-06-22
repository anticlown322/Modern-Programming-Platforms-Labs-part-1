using Faker.Core.DefaultGenerators;
using Faker.Core.Shared;

namespace Faker.Tests;

public class BasicTypesGenTests
{
    private readonly IFaker _systemUnderTest = new Core.Faker();

    #region Bool

    [Fact]
    public void BoolGenerator_Generate_ReturnBool()
    {
        var randomBoolVariable = _systemUnderTest.Create<bool>();

        Assert.IsType<bool>(randomBoolVariable);
    }

    [Fact]
    public void BoolGenerator_CanGenerate_ReturnTrue()
    {
        BoolGenerator gen = new();

        bool result = gen.CanGenerate(typeof(bool));

        Assert.True(result);
    }

    [Fact]
    public void BoolGenerator_CanGenerate_ReturnFalse()
    {
        BoolGenerator gen = new();

        bool result = gen.CanGenerate(typeof(int));

        Assert.False(result);
    }

    #endregion

    #region Byte

    [Fact]
    public void ByteGenerator_Generate_ReturnByte()
    {
        var randomByteVariable = _systemUnderTest.Create<byte>();

        Assert.IsType<byte>(randomByteVariable);
    }

    [Fact]
    public void ByteGenerator_CanGenerate_ReturnTrue()
    {
        ByteGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.True(result);
    }

    [Fact]
    public void ByteGenerator_CanGenerate_ReturnFalse()
    {
        ByteGenerator gen = new();

        bool result = gen.CanGenerate(typeof(int));

        Assert.False(result);
    }

    #endregion

    #region SByte

    [Fact]
    public void SByteGenerator_Generate_ReturnSByte()
    {
        var randomSByteVariable = _systemUnderTest.Create<sbyte>();

        Assert.IsType<sbyte>(randomSByteVariable);
    }

    [Fact]
    public void SByteGenerator_CanGenerate_ReturnTrue()
    {
        SByteGenerator gen = new();

        bool result = gen.CanGenerate(typeof(sbyte));

        Assert.True(result);
    }

    [Fact]
    public void SByteGenerator_CanGenerate_ReturnFalse()
    {
        SByteGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }

    #endregion

    #region Char

    [Fact]
    public void CharGenerator_Generate_ReturnChar()
    {
        var randomCharVariable = _systemUnderTest.Create<char>();

        Assert.IsType<char>(randomCharVariable);
    }

    [Fact]
    public void CharGenerator_CanGenerate_ReturnTrue()
    {
        CharGenerator gen = new();

        bool result = gen.CanGenerate(typeof(char));

        Assert.True(result);
    }

    [Fact]
    public void CharGenerator_CanGenerate_ReturnFalse()
    {
        CharGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }

    #endregion

    #region Decimal

    [Fact]
    public void DecimalGenerator_Generate_ReturnDecimal()
    {
        var randomDecimalVariable = _systemUnderTest.Create<decimal>();

        Assert.IsType<decimal>(randomDecimalVariable);
    }

    [Fact]
    public void DecimalGenerator_CanGenerate_ReturnTrue()
    {
        DecimalGenerator gen = new();

        bool result = gen.CanGenerate(typeof(decimal));

        Assert.True(result);
    }

    [Fact]
    public void DecimalGenerator_CanGenerate_ReturnFalse()
    {
        DecimalGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }

    #endregion

    #region Double

    [Fact]
    public void DoubleGenerator_Generate_ReturnDouble()
    {
        var randomDoubleVariable = _systemUnderTest.Create<double>();

        Assert.IsType<double>(randomDoubleVariable);
    }

    [Fact]
    public void DoubleGenerator_CanGenerate_ReturnTrue()
    {
        DoubleGenerator gen = new();

        bool result = gen.CanGenerate(typeof(double));

        Assert.True(result);
    }

    [Fact]
    public void DoubleGenerator_CanGenerate_ReturnFalse()
    {
        DoubleGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }

    #endregion
    
    #region Decimal

    [Fact]
    public void FloatGenerator_Generate_ReturnFloat()
    {
        var randomFloatVariable = _systemUnderTest.Create<float>();

        Assert.IsType<float>(randomFloatVariable);
    }

    [Fact]
    public void FloatGenerator_CanGenerate_ReturnTrue()
    {
        FloatGenerator gen = new();

        bool result = gen.CanGenerate(typeof(float));

        Assert.True(result);
    }

    [Fact]
    public void FloatGenerator_CanGenerate_ReturnFalse()
    {
        FloatGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }    

    #endregion

    #region Int

    [Fact]
    public void IntGenerator_Generate_ReturnInt()
    {
        var randomIntVariable = _systemUnderTest.Create<int>();

        Assert.IsType<int>(randomIntVariable);
    }

    [Fact]
    public void IntGenerator_CanGenerate_ReturnTrue()
    {
        IntGenerator gen = new();

        bool result = gen.CanGenerate(typeof(int));

        Assert.True(result);
    }

    [Fact]
    public void IntGenerator_CanGenerate_ReturnFalse()
    {
        IntGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }

    #endregion
    
    #region Long

    [Fact]
    public void LongGenerator_Generate_ReturnLong()
    {
        var randomLongVariable = _systemUnderTest.Create<long>();

        Assert.IsType<long>(randomLongVariable);
    }

    [Fact]
    public void LongGenerator_CanGenerate_ReturnTrue()
    {
        LongGenerator gen = new();

        bool result = gen.CanGenerate(typeof(long));

        Assert.True(result);
    }

    [Fact]
    public void LongGenerator_CanGenerate_ReturnFalse()
    {
        LongGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }

    #endregion

    #region String

    [Fact]
    public void StringGenerator_Generate_ReturnString()
    {
        var randomStringVariable = _systemUnderTest.Create<string>();

        Assert.IsType<string>(randomStringVariable);
    }

    [Fact]
    public void StringGenerator_CanGenerate_ReturnTrue()
    {
        StringGenerator gen = new();

        bool result = gen.CanGenerate(typeof(string));

        Assert.True(result);
    }

    [Fact]
    public void StringGenerator_CanGenerate_ReturnFalse()
    {
        StringGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }

    #endregion
}