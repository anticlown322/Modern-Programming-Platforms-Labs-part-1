using Faker.Core.Shared;
using Faker.Core.SpecialGenerators;

namespace Faker.Tests;

public class ClassAndStructGenTests
{
    private readonly IFaker _systemUnderTest = new Core.Faker();

    #region Assets for tests

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class ManyConstuctorsClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }

        public ManyConstuctorsClass()
        {
            Name = "damn don sheesh don baza don uff grrrra brrraa skja";
            Age = default;
            Salary = default;
        }

        public ManyConstuctorsClass(string name)
        {
            Name = name;
        }

        public ManyConstuctorsClass(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public ManyConstuctorsClass(string name, int age, int salary)
        {
            Name = name;
            Age = age;
            Salary = salary;
        }
    }

    public class PrivateContructorClass
    {
        public string Name { get; set; }
        public int Age { get; set; }

        private PrivateContructorClass(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
    
    public struct StructToTest
    {
        public long FieldToFill { get; }

        public StructToTest(long fieldToFill)
        {
            FieldToFill = fieldToFill;
        }
    }
    
    //cyclic dependency
    public class A
    {
        public B B { get; set; }
    }

    public class B
    {
        public C C { get; set; }
    }

    public class C
    {
        public A A { get; set; }
    }

    #endregion

    [Fact]
    public void ClassAndStructGenerator_Generate_ReturnClass()
    {
        var user = _systemUnderTest.Create<User>();

        Assert.NotNull(user);
        Assert.NotNull(user.Name);
        Assert.InRange(user.Age, 1, int.MaxValue);
    }

    [Fact]
    public void ClassAndStructGenerator_Generate_ReturnClass_ManyConstructors()
    {
        var user = _systemUnderTest.Create<ManyConstuctorsClass>();

        Assert.NotNull(user);
        Assert.NotNull(user.Name);
        Assert.True(user.Age != default);
        Assert.True(user.Salary != default);
    }
    
    [Fact]
    public void ClassAndStructGenerator_Generate_ReturnClass_PrivateConstructor()
    {
        var user = _systemUnderTest.Create<PrivateContructorClass>();

        Assert.NotNull(user);
        Assert.NotNull(user.Name);
        Assert.True(user.Age != default);
    }

    [Fact]
    public void ClassAndStructGenerator_Generate_ReturnStruct()
    {
        var randomStructValue = _systemUnderTest.Create<StructToTest>();

        Assert.IsType<StructToTest>(randomStructValue);
        Assert.True(randomStructValue.FieldToFill != default);
    }

    [Fact]
    public void ClassAndStructGenerator_Generate_CyclicDependencyBreak()
    {
        var a = _systemUnderTest.Create<A>();

        Assert.NotNull(a.B);
        Assert.NotNull(a.B.C);
        Assert.Null(a.B.C.A);
    }    
    

    [Fact]
    public void ClassAndStructGenerator_CanGenerate_ReturnTrue()
    {
        ClassAndStructGenerator gen = new();

        bool result = gen.CanGenerate(typeof(User))
                      && gen.CanGenerate(typeof(StructToTest));

        Assert.True(result);
    }

    [Fact]
    public void ClassAndStructGenerator_CanGenerate_ReturnFalse()
    {
        ClassAndStructGenerator gen = new();

        bool result = gen.CanGenerate(typeof(byte));

        Assert.False(result);
    }
}