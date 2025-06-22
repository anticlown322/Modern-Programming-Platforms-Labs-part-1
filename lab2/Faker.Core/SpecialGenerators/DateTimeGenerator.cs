using Faker.Core.Shared;

namespace Faker.Core.SpecialGenerators;

public class DateTimeGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        int year = context.Random.Next(1900, DateTime.Now.Year + 1);
        int month = context.Random.Next(1, 13);
        int day = context.Random.Next(1, DateTime.DaysInMonth(year, month) + 1);
        int hour = context.Random.Next(0, 24);
        int minute = context.Random.Next(0, 60);
        int second = context.Random.Next(0, 60);

        return new DateTime(year, month, day, hour, minute, second);
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(DateTime);
    }
}