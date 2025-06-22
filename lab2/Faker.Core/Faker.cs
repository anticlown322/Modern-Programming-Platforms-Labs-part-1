using System.Reflection;
using Faker.Core.DefaultGenerators;
using Faker.Core.Shared;
using Faker.Core.SpecialGenerators;

namespace Faker.Core;

public class Faker : IFaker
{
    private readonly Random _random;
    private readonly List<IValueGenerator> _generators;
    private readonly FakerConfig _config;

    public Faker(FakerConfig config = null)
    {
        _random = new Random();
        _generators = new List<IValueGenerator>
        {
            new BoolGenerator(),
            new ByteGenerator(),
            new CharGenerator(),
            new DecimalGenerator(),
            new DoubleGenerator(),
            new FloatGenerator(),
            new IntGenerator(),
            new LongGenerator(),
            new SByteGenerator(),
            new StringGenerator(),

            //special types
            new DateTimeGenerator(),
            new EnumerableGenerator(),
            new ClassAndStructGenerator()
        };
        _config = config;
    }

    public T Create<T>()
    {
        var type = typeof(T);
        var context = new GeneratorContext(_random, this);

        return (T)Generate(type, context);
    }

    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var customGenerators = _config?.GetCustomGenerators(typeToGenerate);

        if (customGenerators != null)
        {
            return GenerateWithCustomGenerators(typeToGenerate, context, customGenerators);
        }

        foreach (var generator in _generators)
        {
            if (generator.CanGenerate(typeToGenerate))
            {
                return generator.Generate(typeToGenerate, context);
            }
        }

        throw new InvalidOperationException($"No generator found for type {typeToGenerate}");
    }

    private object GenerateWithCustomGenerators(Type typeToGenerate, GeneratorContext context,
        Dictionary<string, IValueGenerator> customGenerators)
    {
        var constructors = typeToGenerate
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderByDescending(c => c.GetParameters().Length)
            .ToList();

        object instance = null;

        foreach (var constructor in constructors)
        {
            try
            {
                var parameters = GenerateConstructorParameters(constructor, context, customGenerators);
                instance = constructor.Invoke(parameters);
                break; 
            }
            catch
            {
                //just continue
            }
        }

        if (instance == null)
        {
            throw new InvalidOperationException($"Cannot create instance of type {typeToGenerate}");
        }
        
        FillPublicMembers(instance, context, customGenerators);

        return instance;
    }

    private object[] GenerateConstructorParameters(ConstructorInfo constructor, GeneratorContext context, 
        Dictionary<string, IValueGenerator> customGenerators)
    {
        var parameters = constructor.GetParameters();
        var result = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var parameterName = parameter.Name;
            
            if (customGenerators.TryGetValue(parameterName.ToLower(), out var generator))
            {
                result[i] = generator.Generate(parameter.ParameterType, context);
            }
            else
            {
                var createMethod = typeof(IFaker).GetMethod("Create")!.MakeGenericMethod(parameter.ParameterType);
                result[i] = createMethod.Invoke(context.Faker, null);
            }
        }

        return result;
    }
    
    private void FillPublicMembers(object instance, GeneratorContext context,
        Dictionary<string, IValueGenerator> customGenerators)
    {
        var type = instance.GetType();
        
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!field.IsInitOnly) 
            {
                if (customGenerators.TryGetValue(field.Name.ToLower(), out var generator))
                {
                    field.SetValue(instance, generator.Generate(field.FieldType, context));
                }
                else
                {
                    var createMethod = typeof(IFaker).GetMethod("Create")!.MakeGenericMethod(field.FieldType);
                    field.SetValue(instance, createMethod.Invoke(context.Faker, null));
                }
            }
        }
        
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.CanWrite && property.SetMethod != null && property.SetMethod.IsPublic)
            {
                if (customGenerators.TryGetValue(property.Name.ToLower(), out var generator))
                {
                    property.SetValue(instance, generator.Generate(property.PropertyType, context));
                }
                else
                {
                    var createMethod = typeof(IFaker).GetMethod("Create")!.MakeGenericMethod(property.PropertyType);
                    property.SetValue(instance, createMethod.Invoke(context.Faker, null));
                }
            }
        }
    }
}