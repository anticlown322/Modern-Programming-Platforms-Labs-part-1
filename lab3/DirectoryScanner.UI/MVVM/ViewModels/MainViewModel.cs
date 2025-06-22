using System.Windows;
using DirectoryScanner.UI.Core;

namespace DirectoryScanner.UI.MVVM.ViewModels;

public class MainViewModel : ObservableObject
{
    public AboutViewModel AboutVm { get; set; }
    public HelpViewModel HelpVm { get; set; }
    public DirectoryScannerViewModel DirectoryScannerVm { get; set; }

    public RelayCommand MinimizeWindowCommand { get; set; }
    public RelayCommand CloseWindowCommand { get; set; }
    
    public RelayCommand AboutViewCommand { get; set; }
    public RelayCommand HelpViewCommand { get; set; }
    public RelayCommand DirectoryScannerViewCommand { get; set; }

    private object _currentView;
    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        AboutVm = new AboutViewModel();
        HelpVm = new HelpViewModel();
        DirectoryScannerVm = new DirectoryScannerViewModel();

        CurrentView = DirectoryScannerVm; 
        
        MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
        CloseWindowCommand = new RelayCommand(CloseWindow);
        AboutViewCommand = new RelayCommand(obj => CurrentView = AboutVm );
        HelpViewCommand = new RelayCommand(obj => CurrentView = HelpVm );
        DirectoryScannerViewCommand = new RelayCommand(obj => CurrentView = DirectoryScannerVm );
    }
    
    void MinimizeWindow(object parameter)
    {
        if (parameter is Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    void CloseWindow(object parameter)
    {
        if (parameter is Window window)
        {
            window.Close();
        }
    }
}