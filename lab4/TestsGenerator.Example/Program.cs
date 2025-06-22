using TestsGenerator.Backend;

static int ReadPositiveInteger(int defaultValue)
{
    string input = Console.ReadLine()!;
    if (int.TryParse(input, out int result) && result > 0)
    {
        return result;
    }

    Console.WriteLine($"Using default value: {defaultValue}");
    return defaultValue;
}

static void ExitProgram()
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

Console.WriteLine("Test Generator Tool");
Console.WriteLine("==================");

Console.WriteLine("Enter the path to the directory with *.cs files:");
string inputFolder = Console.ReadLine()!;

if (string.IsNullOrWhiteSpace(inputFolder) || !Directory.Exists(inputFolder))
{
    Console.WriteLine("Error: Input folder does not exist.");
    ExitProgram();
    return;
}

Console.WriteLine("Enter the path to the output directory:");
string outputFolder = Console.ReadLine()!;

if (string.IsNullOrWhiteSpace(outputFolder))
{
    Console.WriteLine("Error: Output directory path cannot be null or empty.");
    ExitProgram();
    return;
}

if (!Directory.Exists(outputFolder))
{
    Directory.CreateDirectory(outputFolder);
    Console.WriteLine($"Created output directory: {outputFolder}");
}

var inputFiles = Directory.GetFiles(inputFolder, "*.cs", SearchOption.AllDirectories);
if (inputFiles.Length == 0)
{
    Console.WriteLine("No input files found.");
    ExitProgram();
    return;
}

Console.WriteLine($"{inputFiles.Length} files were found. Starting test generation...");

Console.WriteLine("Enter the maximum number of files to load at once (default: 5):");
int maxFilesToLoad = ReadPositiveInteger(5);

Console.WriteLine("Enter the maximum number of files to process at once (default: 10):");
int maxFilesToProcess = ReadPositiveInteger(10);

Console.WriteLine("Enter the maximum number of files to write at once (default: 5):");
int maxFilesToWrite = ReadPositiveInteger(5);

var generator = new TestGenerator(maxFilesToLoad, maxFilesToProcess, maxFilesToWrite);

try
{
    await generator.GenerateTestsAsync(inputFiles, outputFolder);
    Console.WriteLine("Test generation completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during test generation: {ex.Message}");
}

ExitProgram();

