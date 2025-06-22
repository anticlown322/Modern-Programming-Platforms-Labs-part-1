using System.Diagnostics;
using DirectoryScanner.UI.Core;

namespace DirectoryScanner.UI.MVVM.ViewModels;

public class AboutViewModel : ObservableObject
{
    public RelayCommand GithubPageCommand { get; set; }
    public RelayCommand GithubProfileCommand { get; set; }
    public RelayCommand LinkedinCommand { get; set; }
    public AboutViewModel()
    {
        GithubPageCommand = new RelayCommand(FollowGithubPageLink);
        GithubProfileCommand = new RelayCommand(FollowGithubProfileLink);
        LinkedinCommand = new RelayCommand(FollowLinkedinLink);
    }
    
    void FollowGithubPageLink(object parameter)
    {
        Process.Start(new ProcessStartInfo("https://github.com/anticlown322/Modern-Programming-Platforms-Lab3-Karas") 
            { UseShellExecute = true });
    }

    void FollowGithubProfileLink(object parameter)
    {
        Process.Start(new ProcessStartInfo("https://github.com/anticlown322") { UseShellExecute = true });
    }

    void FollowLinkedinLink(object parameter)
    {
        Process.Start(new ProcessStartInfo("https://www.linkedin.com/in/andreykaras/") { UseShellExecute = true });
    }
}