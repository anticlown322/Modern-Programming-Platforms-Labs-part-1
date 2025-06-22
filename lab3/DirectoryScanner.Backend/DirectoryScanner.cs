using System.Collections.Concurrent;

namespace DirectoryScanner.Backend;

public class DirectoryScanner
{
    private static readonly int MaxThreads = Environment.ProcessorCount;

    public DirectoryNode Scan(string rootPath, CancellationToken cancellationToken)
    {
        var root = new DirectoryNode(rootPath);
        var queue = new ConcurrentQueue<DirectoryNode>();
        queue.Enqueue(root);

        var countdown = new CountdownEvent(1);
        var consumers = new Task[MaxThreads];
        
        for (int i = 0; i < MaxThreads; i++)
        {
            consumers[i] = Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!queue.TryDequeue(out var dir)) break;

                    try
                    {
                        ProcessDirectory(dir, queue, countdown, cancellationToken);
                    }
                    finally
                    {
                        countdown.Signal();
                    }
                }
            }, cancellationToken);
        }


        try
        {
            countdown.Wait(cancellationToken);
            Task.WaitAll(consumers, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            //
        }
        finally
        {
            countdown.Dispose();
        }
        
        CalcDirSize(root);

        return root;
    }
    
    private static void ProcessDirectory(
        DirectoryNode dir, 
        ConcurrentQueue<DirectoryNode> queue, 
        CountdownEvent countdown, 
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        try
        {
            var directoryInfo = new DirectoryInfo(dir.FullPath);

            foreach (var entry in directoryInfo.EnumerateFileSystemInfos())
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                if (entry.LinkTarget != null)
                    continue;

                switch (entry)
                {
                    case FileInfo file:
                    {
                        dir.Files.Add(new(file.Name, file.Length));
                        dir.FileSize += file.Length;
                        break;
                    }
                    
                    case DirectoryInfo subDir:
                    {
                        var subDirNode = new DirectoryNode(subDir.FullName) { Parent = dir };
                        dir.Subdirectories.Add(subDirNode);
                        queue.Enqueue(subDirNode);
                        countdown.AddCount(1);
                        break;
                    }
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            //
        }
    }
    
    private void CalcDirSize(DirectoryNode? node)
    {
        if (node == null)
            return;

        foreach (var subDir in node.Subdirectories)
            CalcDirSize(subDir);

        node.TotalSize = node.FileSize + node.Subdirectories.Sum(d => d.TotalSize);
    }
}