using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestsGenerator.Backend;

namespace TestsGenerator.Tests;

[TestFixture]
public class TestGeneratorTests
{
    [Test]
    public async Task GenerateTestsAsync_ShouldGenerateCorrectTestClass()
    {
        #region Arrange

        var inputFiles = new List<string> { "Input.cs" };
        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "TestOutput");
        Directory.CreateDirectory(outputPath);

        var tempInputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Input.cs");
        var inputCode = @"
                        public class Calculator
                        {
                            public int Add(int a, int b) => a + b;
                            public int Subtract(int a, int b) => a - b;
                        }";
        await File.WriteAllTextAsync(tempInputFilePath, inputCode);
        inputFiles.Add(tempInputFilePath);
        
        
        var generator = new TestGenerator(maxFilesToLoad: 1, maxFilesToProcess: 1, maxFilesToWrite: 1);

        #endregion Arrange

        #region Act

        await generator.GenerateTestsAsync(inputFiles, outputPath);

        #endregion Act

        #region Assert

        var generatedFilePath = Path.Combine(outputPath, $"{TestClassName}.cs");
        Assert.IsTrue(File.Exists(generatedFilePath));

        var generatedContent = await File.ReadAllTextAsync(generatedFilePath);
        Assert.IsTrue(generatedContent.Contains($"public class {TestClassName}"));
        Assert.IsTrue(generatedContent.Contains($"public void {TestMethodName1}()"));
        Assert.IsTrue(generatedContent.Contains($"public void {TestMethodName2}()"));
        Assert.IsTrue(generatedContent.Contains("Assert.Fail(\"damn it's autogen bullshit\");"));

        #endregion Assert

        #region Cleanup

        Directory.Delete(outputPath, recursive: true);

        #endregion Cleanup
    }

    private const string TestClassName = "CalculatorTests";
    private const string TestMethodName1 = "TestAdd";
    private const string TestMethodName2 = "TestSubtract";

    [Test]
    public void GenerateTestMethods_ShouldGenerateCorrectTestMethods()
    {
        #region Arrange

        var inputCode = @"
                        public class Calculator
                        {
                            public int Add(int a, int b) => a + b;
                            public int Subtract(int a, int b) => a - b;
                        }";
        var tree = CSharpSyntaxTree.ParseText(inputCode);
        var root = tree.GetRoot();
        var classDecl = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var generator = new TestGenerator(maxFilesToLoad: 1, maxFilesToProcess: 1, maxFilesToWrite: 1);

        #endregion Arrange

        #region Act

        var testMethods = generator.GenerateTestMethods(classDecl).ToList();

        #endregion Act

        #region Assert

        Assert.That(testMethods.Count, Is.EqualTo(2));
        Assert.IsTrue(testMethods.Any(m => m.Contains($"public void {TestMethodName1}()")));
        Assert.IsTrue(testMethods.Any(m => m.Contains($"public void {TestMethodName2}()")));
        Assert.IsTrue(testMethods.All(m => m.Contains("Assert.Fail(\"damn it's autogen bullshit\");")));

        #endregion Assert
    }
}