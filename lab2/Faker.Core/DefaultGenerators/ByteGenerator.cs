using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class ByteGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        byte[] arr = new byte[1];
        context.Random.NextBytes(arr);
        return arr[0];
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(byte);
    }
}