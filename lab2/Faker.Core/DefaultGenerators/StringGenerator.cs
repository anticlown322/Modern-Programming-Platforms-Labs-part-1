using Faker.Core.Shared;

namespace Faker.Core.DefaultGenerators;

public class StringGenerator : IValueGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var length = context.Random.Next(5, 15);
        return new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[context.Random.Next(s.Length)]).ToArray());
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(string);
    }
}