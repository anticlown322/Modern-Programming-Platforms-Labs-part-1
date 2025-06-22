using Faker.Core;

var faker = new Faker.Core.Faker();

var empl = faker.Create<Employee>();
var empl2 = faker.Create<Employee>();

var notEmpl = faker.Create<NotEmployee>();
var notEmpl2 = faker.Create<NotEmployee>();

Console.WriteLine("\t\t**Employees**");
Console.WriteLine(empl.ToString());
Console.WriteLine(empl2.ToString());

Console.WriteLine("\n\t\t**NotEmployees**");
Console.WriteLine(notEmpl.ToString());
Console.WriteLine(notEmpl2.ToString());

//**********************************************

public class NotEmployee
{
    public float aaa { get; set; }
    public bool bbb { get; set; }
    public byte bbb2 { get; set; }

    public string ToString()
    {
        return $"aaa: {aaa}, bbb: {bbb}, bbb2: {bbb2}";
    }
}

public class Employee
{
    public string Name;  
    public DateTime DateOfJoining { get; set;}
    public NotEmployee NotEmployee { get; set; }
    
    public override string ToString()
    {
        return $"Name: {Name}, DateOfJoining: {DateOfJoining}, \n\t -> NotEmlpoe:   " + NotEmployee.ToString();
    }
}