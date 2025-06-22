namespace DirectoryScanner.Tests;

public class DirectoryScannerTests
{
    private readonly string _testRoot = Path.Combine(Directory.GetCurrentDirectory(), "tests");

    [Fact]
    public void Scan_EmptyDirectory_ReturnValidStructure()
    {
        #region Arrange

        var scanner = new Backend.DirectoryScanner();
        var dirPath = Path.Combine(_testRoot, "dir");
        Directory.CreateDirectory(dirPath);

        #endregion Arrange

        #region Act

        var result = scanner.Scan(dirPath, CancellationToken.None);

        #endregion Assert

        #region MyRegion

        Assert.Equal(dirPath, result.FullPath);
        Assert.Empty(result.Files);
        Assert.Empty(result.Subdirectories);
        Assert.Equal(0L, result.TotalSize);
        CleanupTestDir();
        
        #endregion Assert
    }

    [Theory]
    [InlineData("damn", "sheesh")]
    [InlineData("skrrr", "brrrrrrrrr")]
    [InlineData("smoking", "zaza")]
    public void Scan_DirectoryWithFiles_ReturnCorrectSize(string str1, string str2)
    {
        #region Arrange

        var scanner = new Backend.DirectoryScanner();
        var dirPath = Path.Combine(_testRoot, "dir");
        Directory.CreateDirectory(dirPath);
            
        File.WriteAllText(Path.Combine(dirPath, "file1.txt"), str1);
        File.WriteAllText(Path.Combine(dirPath, "file2.txt"), str2);

        #endregion Arrange

        #region Act

        var result = scanner.Scan(dirPath, CancellationToken.None);

        #endregion Act

        #region Assert

        Assert.Equal(2, result.Files.Count);
        Assert.Equal(str1.Length + str2.Length, result.TotalSize); 
        CleanupTestDir();
        
        #endregion Assert
    }

    [Theory]
    [InlineData("damn", "sheesh")]
    [InlineData("skrrr", "brrrrrrrrr")]
    [InlineData("smoking", "zaza")]
    public void Scan_NestedDirectories_ReturnWithCorrectNesting(string str1, string str2)
    {
        #region Arrange

        var scanner = new Backend.DirectoryScanner();
        var dirPath = Path.Combine(_testRoot, "dir");
        Directory.CreateDirectory(dirPath);
            
        var subDir = Path.Combine(dirPath, "nested");
        Directory.CreateDirectory(subDir);
            
        File.WriteAllText(Path.Combine(dirPath, "root.txt"), str1);
        File.WriteAllText(Path.Combine(subDir, "nested.txt"), str2);
        
        #endregion Arrange

        #region Act

        var result = scanner.Scan(dirPath, CancellationToken.None);

        #endregion Act

        #region Assert

        Assert.Single(result.Files);
        Assert.Single(result.Subdirectories);
        Assert.Equal(str1.Length + str2.Length, result.TotalSize); 

        var subDirNode = result.Subdirectories[0];
        Assert.Single(subDirNode.Files);
        Assert.Equal(str2.Length , subDirNode.TotalSize);

        CleanupTestDir();

        #endregion Assert
    }

    [Theory]
    [InlineData(25)]
    [InlineData(50)]
    [InlineData(100)]
    public void Scan_Cancellation_ReturnPartialResults(int numOfFiles)
    {
        #region Arrange

        var testStr = "damn";
        var scanner = new Backend.DirectoryScanner();
        var cts = new CancellationTokenSource();
        var dirPath = Path.Combine(_testRoot, "dir");
        Directory.CreateDirectory(dirPath); 

        for (int i = 0; i < numOfFiles; i++)
            File.WriteAllText(Path.Combine(dirPath, $"{i}.txt"), testStr);
        
        cts.CancelAfter(1);
        
        #endregion Arrange

        #region Act

        var result = scanner.Scan(dirPath, cts.Token);

        #endregion Act

        #region Assert

        Assert.True(result.Files.Count <= numOfFiles);
        Assert.True(result.TotalSize <= testStr.Length * numOfFiles);

        CleanupTestDir();
        
        #endregion Assert
    }
    
    public DirectoryScannerTests()
    {
        Directory.CreateDirectory(_testRoot);
    }

    private void CleanupTestDir()
    {
        if (Directory.Exists(_testRoot))
            Directory.Delete(_testRoot, true);
    }
}