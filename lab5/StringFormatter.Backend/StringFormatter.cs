using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text;

namespace StringFormatter.Backend;

public class StringFormatter : IStringFormatter
{
    public static readonly StringFormatter Shared = new();

    private readonly ConcurrentDictionary<string, Delegate> _cache = new();

    public string Format(string template, object target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        if (string.IsNullOrEmpty(template))
            return template;

        CheckBalancedBraces(template);

        var result = new StringBuilder();
        int i = 0;
        while (i < template.Length)
        {
            if (template[i] == '{')
            {
                if (i + 1 < template.Length && template[i + 1] == '{')
                {
                    result.Append('{');
                    i += 2;
                }
                else
                {
                    int j = i + 1;
                    while (j < template.Length && template[j] != '}')
                        j++;

                    if (j >= template.Length)
                        throw new FormatException("Unbalanced braces in template.");

                    string propertyName = template.Substring(i + 1, j - i - 1);
                    result.Append(GetPropertyValue(target, propertyName));
                    i = j + 1;
                }
            }
            else if (template[i] == '}')
            {
                if (i + 1 < template.Length && template[i + 1] == '}')
                {
                    result.Append('}');
                    i += 2;
                }
                else
                {
                    throw new FormatException("Unbalanced braces in template.");
                }
            }
            else
            {
                result.Append(template[i]);
                i++;
            }
        }

        return result.ToString();
    }

    private void CheckBalancedBraces(string template)
    {
        int balance = 0;
        for (int i = 0; i < template.Length; i++)
        {
            if (template[i] == '{')
                balance++;
            else if (template[i] == '}')
                balance--;

            if (balance < 0)
                throw new FormatException("Unbalanced braces in template.");
        }

        if (balance != 0)
            throw new FormatException("Unbalanced braces in template.");
    }

    private string GetPropertyValue(object target, string propertyName)
    {
        var key = $"{target.GetType().FullName}.{propertyName}";
        if (!_cache.TryGetValue(key, out var accessor))
        {
            var parameter = Expression.Parameter(target.GetType(), "x");
            MemberExpression property;
            try
            {
                property = Expression.PropertyOrField(parameter, propertyName);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Property or field '{propertyName}' not found on type '{target.GetType().FullName}'.", ex);
            }
            var lambda = Expression.Lambda(property, parameter);
            accessor = lambda.Compile();
            _cache[key] = accessor;
        }

        return accessor.DynamicInvoke(target)?.ToString() ?? string.Empty;
    }
}