using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MileageTrackerMobile.ViewModels;

namespace MileageTrackerMobile.Modules;

public partial class SettingsButton : UserControl
{
    public SettingsButton()
    {
        InitializeComponent();
    }
    
    public async void OpenSettings_OnClick(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as MainViewModel;
        if (vm == null) return;

        vm.LogsPageVisible = false;
        vm.SettingsPageVisible = true;
        vm.SettingsButtonVisible = false;
        vm.ExitButtonVisible = true;
    }
}