using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class BoolGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.Next(0, 2) == 0; // 0 is included, 2 is excluded 
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(bool);
    }
}