using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestsGenerator.Backend;

public class TestGenerator
{
    private readonly int _maxFilesToLoad;
    private readonly int _maxFilesToProcess;
    private readonly int _maxFilesToWrite;

    public TestGenerator(int maxFilesToLoad, int maxFilesToProcess, int maxFilesToWrite)
    {
        _maxFilesToLoad = maxFilesToLoad;
        _maxFilesToProcess = maxFilesToProcess;
        _maxFilesToWrite = maxFilesToWrite;
    }

    public async Task GenerateTestsAsync(IEnumerable<string> inputFiles, string outputPath)
    {
        var loadOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxFilesToLoad };
        var processOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxFilesToProcess };
        var writeOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxFilesToWrite };

        var loadBlock = new TransformBlock<string, string>(
            async filePath => await File.ReadAllTextAsync(filePath),
            loadOptions);

        var processBlock = new TransformManyBlock<string, (string fileName, string content)>(
            fileContent => GenerateTestClasses(fileContent),
            processOptions);

        var writeBlock = new ActionBlock<(string fileName, string content)>(
            async file => await File.WriteAllTextAsync(
                Path.Combine(outputPath, file.fileName), file.content),
            writeOptions);

        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
        loadBlock.LinkTo(processBlock, linkOptions);
        processBlock.LinkTo(writeBlock, linkOptions);   

        foreach (var file in inputFiles) loadBlock.Post(file);
        
        loadBlock.Complete();

        await writeBlock.Completion;
    }

    public IEnumerable<(string fileName, string content)> GenerateTestClasses(string fileContent)
    {
        var tree = CSharpSyntaxTree.ParseText(fileContent);
        var root = tree.GetRoot();

        var classes = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(c => c.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)));

        foreach (var classDecl in classes)
        {
            var className = classDecl.Identifier.Text;
            var testClassName = $"{className}Tests";
            var testMethods = GenerateTestMethods(classDecl);

            var testClassContent = $$"""
                                     using NUnit.Framework;

                                     [TestFixture]
                                     public class {{testClassName}}
                                     {
                                         {{string.Join("\n", testMethods)}}
                                     }
                                     """;

            yield return ($"{testClassName}.cs", testClassContent);
        }
    }

    public IEnumerable<string> GenerateTestMethods(ClassDeclarationSyntax classDecl)
    {
        var methods = classDecl
            .DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .Where(m => m.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PublicKeyword)));

        var methodGroups = methods.GroupBy(m => m.Identifier.Text);

        foreach (var group in methodGroups)
        {
            if (group.Count() == 1)
            {
                var method = group.First();
                var testMethodName = $"Test{method.Identifier.Text}";
                yield return GenerateTestMethod(testMethodName);
            }
            else
            {
                int overloadIndex = 0;
                foreach (var method in group)
                {
                    var parameterTypes = method.ParameterList.Parameters
                        .Select(p => p.Type?.ToString())
                        .ToList();

                    var testMethodName = $"Test{method.Identifier.Text}_{string.Join("_", parameterTypes)}";
                    yield return GenerateTestMethod(testMethodName);
                    overloadIndex++;
                }
            }
        }
    }

    private string GenerateTestMethod(string testMethodName)
    {
        return $$"""

                 [Test]
                 public void {{testMethodName}}()
                 {
                     Assert.Fail("damn it's autogen bullshit");
                 }
                 """;
    }
}