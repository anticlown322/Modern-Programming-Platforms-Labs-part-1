using Faker.Core.Shared;

namespace Faker.Core.SpecialGenerators;

public class EnumerableGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        Type elementType = typeToGenerate.GetGenericArguments()[0];

        var list = (System.Collections.IList)Activator
            .CreateInstance(typeof(List<>).MakeGenericType(elementType));

        int count = context.Random.Next(1, 11);

        // get Create<T> via reflection
        var createMethod = typeof(IFaker).GetMethod("Create")!.MakeGenericMethod(elementType);

        for (int i = 0; i < count; i++)
        {
            var element = createMethod.Invoke(context.Faker, null);
            list.Add(element);
        }

        if (typeToGenerate.IsArray)
        {
            var array = Array.CreateInstance(elementType, list.Count);
            list.CopyTo(array, 0);
            return array;
        }

        if (typeToGenerate.IsGenericType)
        {
            return list;
        }

        throw new InvalidOperationException($"Unsupported type: {typeToGenerate}");
    }

    public bool CanGenerate(Type type)
    {
        return type.IsArray ||
               (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                 type.GetGenericTypeDefinition() == typeof(IList<>) ||
                 type.GetGenericTypeDefinition() == typeof(ICollection<>)));
    }
}