using System.Reflection;
using Faker.Core.Shared;

namespace Faker.Core.SpecialGenerators;

public class ClassAndStructGenerator : IValueGenerator
{
    //for checking cyclic dependencies
    private readonly Stack<Type> _typeStack = new();

    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        // check cyclic dependency 
        if (_typeStack.Contains(typeToGenerate))
        {
            return null; //return null to avoid infinite recursion
        }

        _typeStack.Push(typeToGenerate);

        try
        {
            // if struct
            if (typeToGenerate.IsValueType && !typeToGenerate.IsPrimitive && !typeToGenerate.IsEnum)
            {
                return GenerateStruct(typeToGenerate, context);
            }

            // else class
            return GenerateClass(typeToGenerate, context);
        }
        finally
        {
            _typeStack.Pop();
        }
    }

    private object GenerateStruct(Type structType, GeneratorContext context)
    {
        // find constructor with max number of params   
        var constructors = structType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderByDescending(c => c.GetParameters().Length)
            .ToList();

        foreach (var constructor in constructors)
        {
            try
            {
                var parameters = GenerateParameters(constructor.GetParameters(), context);
                return constructor.Invoke(parameters);
            }
            catch
            {
                //just continue
            }
        }

        // if no constructor succeeded then use activator for creation
        return Activator.CreateInstance(structType);
    }

    private object GenerateClass(Type classType, GeneratorContext context)
    {
        // find constructor with max number of params   
        var constructors = classType
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderByDescending(c => c.GetParameters().Length)
            .ToList();

        object instance = null;

        foreach (var constructor in constructors)
        {
            try
            {
                var parameters = GenerateParameters(constructor.GetParameters(), context);
                instance = constructor.Invoke(parameters);
                break; // if constructor succeeded end this
            }
            catch
            {
                continue;
            }
        }

        // if no constructor succeeded then try private one
        if (instance == null)
        {
            var privateConstructor = classType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

            if (privateConstructor != null)
            {
                try
                {
                    var parameters = GenerateParameters(privateConstructor.GetParameters(), context);
                    instance = privateConstructor.Invoke(parameters);
                }
                catch
                {
                    // if private constructor did not succeed
                    throw new InvalidOperationException($"Cannot create instance of type {classType}");
                }
            }
            else
            {
                // if there is no contructor at all
                throw new InvalidOperationException($"No suitable constructor found for type {classType}");
            }
        }

        // Заполняем публичные поля и свойства
        FillPublicMembers(instance, context);

        return instance;
    }

    private object[] GenerateParameters(ParameterInfo[] parameters, GeneratorContext context)
    {
        var result = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            var createMethod = typeof(IFaker).GetMethod("Create")!.MakeGenericMethod(parameters[i].ParameterType);
            result[i] = createMethod.Invoke(context.Faker, null);
        }

        return result;
    }

    private void FillPublicMembers(object instance, GeneratorContext context)
    {
        var type = instance.GetType();

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!field.IsInitOnly) // skip readonly field
            {
                try
                {
                    var createMethod = typeof(IFaker).GetMethod("Create")!.MakeGenericMethod(field.FieldType);
                    var value = createMethod.Invoke(context.Faker, null);
                    field.SetValue(instance, value);
                }
                catch
                {
                    // if can't fill the field then just skip it
                }
            }
        }

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.CanWrite && property.SetMethod != null && property.SetMethod.IsPublic)
            {
                try
                {
                    var createMethod = typeof(IFaker).GetMethod("Create")!.MakeGenericMethod(property.PropertyType);
                    var value = createMethod.Invoke(context.Faker, null);
                    property.SetValue(instance, value);
                }
                catch
                {
                    // if can't fill the field then just skip it
                }
            }
        }
    }

    public bool CanGenerate(Type type)
    {
        return type.IsClass ||
               (type.IsValueType && !type.IsPrimitive && !type.IsEnum);
    }
}