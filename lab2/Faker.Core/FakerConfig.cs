using System.Linq.Expressions;
using Faker.Core.Shared;

namespace Faker.Core;

public class FakerConfig
{
    private readonly Dictionary<Type, Dictionary<string, IValueGenerator>> _customGenerators = new();

    public void Add<T, TProperty, TGenerator>(Expression<Func<T, TProperty>> expression)
        where TGenerator : IValueGenerator, new()
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("Expression must be a property or field access.", nameof(expression));
        }

        //  to lower is important because it used in later comparisons like:
        //  customGenerators.TryGetValue(parameterName, out var generator)
        //  where parameterName is lowercase
        var propertyName = memberExpression.Member.Name.ToLower(); 
        var type = typeof(T);

        if (!_customGenerators.ContainsKey(type))
        {
            _customGenerators[type] = new Dictionary<string, IValueGenerator>();
        }

        _customGenerators[type][propertyName] = new TGenerator();
    }

    public Dictionary<string, IValueGenerator> GetCustomGenerators(Type type)
    {
        if (_customGenerators.TryGetValue(type, out var propertyGenerators))
        {
            return propertyGenerators;
        }

        return null;
    }
}