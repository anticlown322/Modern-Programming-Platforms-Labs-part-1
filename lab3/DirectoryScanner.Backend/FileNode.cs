namespace DirectoryScanner.Backend;

public class FileNode
{
    public string Name { get; }
    public long Size { get; }

    public FileNode(string name, long size)
    {
        Name = name;
        Size = size;
    }
}