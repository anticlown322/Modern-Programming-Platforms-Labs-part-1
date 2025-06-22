using Faker.Core;
using Faker.Core.Shared;

namespace Faker.Tests;

public class FakerTests
{
    private readonly IFaker _faker = new Core.Faker();
    
    #region Assets for testing
    
    //just custom generator
    public class Foo
    {
        public string City { get; set; } // Публичное свойство
        public int Population; // Публичное поле
    }

    public class CityGenerator : IValueGenerator
    {
        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            return "Grodno";
        }

        public bool CanGenerate(Type type)
        {
            return type == typeof(string);
        }
    }

    public class PopulationGenerator : IValueGenerator
    {
        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            return context.Random.Next(1000, 1000000);
        }

        public bool CanGenerate(Type type)
        {
            return type == typeof(int);
        }
    }

    public class Person
    {
        public string Name { get; }

        public Person(string name)
        {
            Name = name;
        }
    }

    public class NameGenerator : IValueGenerator
    {
        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            return "Andrei Karas";
        }

        public bool CanGenerate(Type type)
        {
            return type == typeof(string);
        }
    }
    
    #endregion
    
    [Fact]
    public void Faker_Create_ReturnSmth()
    {
        IEnumerable<int> ints = _faker.Create<IEnumerable<int>>();
        
        Assert.NotNull(ints);
        Assert.NotEmpty(ints);
    }
    
    [Fact]
    public void Faker_Create_ReturnException_NoAppropriateGenerator()
    {
        Action act = () => _faker.Create<short>();
        
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Faker_Create_ReturnFooClassWithNotEmptyField_CustomGenerator()
    {
        #region Arrange

        var config = new FakerConfig();
        config.Add<Foo, string, CityGenerator>(foo => foo.City);
        config.Add<Foo, int, PopulationGenerator>(foo => foo.Population);
        var faker = new Core.Faker(config);

        #endregion
        
        #region Act
        
        var foo = faker.Create<Foo>();
        
        #endregion

        #region Assert
        
        Assert.Equal("Grodno", foo.City);
        Assert.InRange(foo.Population, 1000, 1000000-1);
        
        #endregion
    }
    
    [Fact]
    public void Faker_Create_ReturnFooClassWithImmutableField_CustomGenerator()
    {
        #region Arrange

        var config = new FakerConfig();
        config.Add<Person, string, NameGenerator>(p => p.Name);
        var faker = new Core.Faker(config);

        #endregion
        
        #region Act
        
        var person = faker.Create<Person>();
        
        #endregion

        #region Assert
        
        Assert.Equal("Andrei Karas", person.Name);
        
        #endregion
    }
}