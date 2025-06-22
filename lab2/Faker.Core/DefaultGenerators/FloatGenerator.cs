using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class FloatGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        float minValue = -1000000.0f; 
        float maxValue = 1000000.0f;  
        float range = maxValue - minValue;

        double sample = context.Random.NextDouble();
        float result = (float)(sample * range) + minValue;

        return result;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(float);
    }
}