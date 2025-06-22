namespace StringFormatter.Backend;

public interface IStringFormatter
{
    string Format(string template, object target);
}