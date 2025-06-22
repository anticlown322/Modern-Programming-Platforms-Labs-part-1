using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class DecimalGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        byte scale = (byte)context.Random.Next(29);
        bool sign = context.Random.Next(2) == 1;
        
        return new decimal(
            context.Random.Next(int.MinValue, int.MaxValue),
            context.Random.Next(int.MinValue, int.MaxValue),
            context.Random.Next(int.MinValue, int.MaxValue),
            sign,
            scale
        );
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(decimal);
    }
}