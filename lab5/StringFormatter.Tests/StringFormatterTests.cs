using StringFormatter.Backend;

namespace StringFormatter.Tests;

public class StringFormatterTests
{
    class User(string firstName, string lastName)
    {
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
    }
    
    private readonly IStringFormatter _formatter = new Backend.StringFormatter();

    [Fact]
    public void Format_ValidTemplate_ReturnsFormattedString()
    {
        var user = new User("Damn", "Sheesh");
        var result = _formatter.Format("q, {FirstName} {LastName}!", user);
        Assert.Equal("q, Damn Sheesh!", result);
    }

    [Fact]
    public void Format_EscapedBraces_ReturnsCorrectString()
    {
        var user = new User("Damn", "Sheesh");
        var result = _formatter.Format("{{FirstName}} -> {FirstName}", user);
        Assert.Equal("{FirstName} -> Damn", result);
    }

    [Fact]
    public void Format_UnbalancedBraces_ThrowsException()
    {
        var user = new User("Damn", "Sheesh");
        Assert.Throws<FormatException>(() => _formatter.Format("q, {FirstName {LastName}!", user));
    }

    [Fact]
    public void Format_NonExistentProperty_ThrowsException()
    {
        var user = new User("Damn", "Sheesh");
        Assert.Throws<InvalidOperationException>(() => _formatter.Format("q, {NonExistentProperty}!", user));
    }

    [Fact]
    public void Format_NullTarget_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _formatter.Format("q, {FirstName}!", null));
    }

    [Fact]
    public void Format_EmptyTemplate_ReturnsEmptyString()
    {
        var user = new User("Damn", "Sheesh");
        var result = _formatter.Format("", user);
        Assert.Equal("", result);
    }

    [Fact]
    public void Format_NonStringProperty_ReturnsToStringValue()
    {
        var obj = new { Number = 42 };
        var result = _formatter.Format("Value: {Number}", obj);
        Assert.Equal("Value: 42", result);
    }
}