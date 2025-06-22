using DirectoryScanner.Backend;
using DirectoryScanner.UI.Core;

namespace DirectoryScanner.UI.MVVM.ViewModels;

public class FileNodeViewModel : ObservableObject
{
    private readonly FileNode _model;
    private readonly DirectoryNodeViewModel? _parent;

    public string Name => _model.Name;
    public string SizeText => $"{_model.Size:N0} bytes";
    public string PercentageText => $"{Percentage:F1}%";
    public double Percentage => CalculatePercentage();

    public FileNodeViewModel(FileNode model, DirectoryNodeViewModel parent)
    {
        _model = model;
        _parent = parent;
    }

    private double CalculatePercentage()
    {
        if (_parent == null || _parent.Model.TotalSize == 0)
            return 0;
        else
            return (double)_model.Size / _parent.Model.TotalSize * 100;
    }
}