using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class CharGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return (char)context.Random.Next(char.MinValue, char.MaxValue + 1);
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(char);
    }
}