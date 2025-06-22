using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class SByteGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return (sbyte)context.Random.Next(sbyte.MinValue, sbyte.MaxValue + 1);
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(sbyte);
    }
}