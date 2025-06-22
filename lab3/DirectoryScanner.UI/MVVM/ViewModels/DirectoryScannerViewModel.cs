using System.Windows.Input;
using DirectoryScanner.Backend;
using DirectoryScanner.UI.Core;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DirectoryScanner.UI.MVVM.ViewModels;

public class DirectoryScannerViewModel : ObservableObject
{
    private DirectoryNodeViewModel? _rootNode;
    private CancellationTokenSource _cts;

    public DirectoryNodeViewModel? RootNode
    {
        get => _rootNode;
        set
        {
            _rootNode = value;
            OnPropertyChanged();
        }
    }

    public ICommand SelectAndStartCommand { get; }
    public ICommand CancelCommand { get; }

    public DirectoryScannerViewModel()
    {
        SelectAndStartCommand = new RelayCommand(async (object _) => await SelectAndStart());
        CancelCommand = new RelayCommand(Cancel);
    }

    private async Task SelectAndStart()
    {
        using var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        dialog.InitialDirectory = Environment.CurrentDirectory;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            _cts = new CancellationTokenSource();

            DirectoryNode? root = null;

            try
            {
                root = await Task.Run(() => new Backend.DirectoryScanner().Scan(dialog.FileName!, _cts.Token));
            }
            catch (OperationCanceledException)
            {
                //
            }

            RootNode = root != null ? new(root, null!) : null;
        }
    }

    private void Cancel(object parameter)
    {
        _cts.Cancel();
    }
}