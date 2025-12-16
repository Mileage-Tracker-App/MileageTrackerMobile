using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MileageTrackerMobile.ViewModels;

namespace MileageTrackerMobile.Modules;

public partial class UserIdDisplay : UserControl
{
    public UserIdDisplay()
    {
        InitializeComponent();
    }
    
    public async void Continue_OnClick(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as MainViewModel;
        if (vm == null) return;

        vm.SessionIdDisplayVisible = false;
        vm.LoginScreenVisible = false;
        vm.LogsPageVisible = true;
        vm.SettingsButtonVisible = true;

    }
}