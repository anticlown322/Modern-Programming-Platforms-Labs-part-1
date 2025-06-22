using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class LongGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        byte[] buffer = new byte[8];
        context.Random.NextBytes(buffer);
        return BitConverter.ToInt64(buffer, 0);
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(long);
    }
}