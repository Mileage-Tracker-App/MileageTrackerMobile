using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MileageTrackerMobile.Models;
using MileageTrackerMobile.ViewModels;

namespace MileageTrackerMobile.Modules;

public partial class LoginScreen : UserControl
{
    public LoginScreen()
    {
        InitializeComponent();
    }
    
    public void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public async void SignUp_OnClick(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as MainViewModel;
        if (vm == null) return;
            
        var result = await vm._apiController.CreateSessionAsync<TSession>();
        if (result.Success && result.Value != null)
        {

            vm.SessionID = result.Value.Id;
            vm.sessionID = result.Value.Id;
            vm.SessionIdDisplayVisible = true;

        }
    }
    
    public async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm == null) return;
            
            if (string.IsNullOrWhiteSpace(vm.loginInput))
            {
                vm.SessionNotExistVisible = true;
                await Task.Delay(2000);
                vm.SessionNotExistVisible = false;
                return;
            }
            
            if (int.TryParse(vm.loginInput, out var sessionId))
            {
                var result = await vm._apiController.GetSessionAsync<TSession>(sessionId);

                if (result.Success && result.Value != null)
                {
                    vm.SessionID = result.Value.Id;
                    vm.sessionID = result.Value.Id;
                    vm.LoginScreenVisible = false;
                    vm.LogsPageVisible = true;
                    vm.SettingsButtonVisible = true;
                }
                else
                {
                    vm.SessionNotExistVisible = true;
                    await Task.Delay(2000);
                    vm.SessionNotExistVisible = false;
                }
            }
            else
            {
                var result = await vm._apiController.CreateSessionAsync<TSession>();

                if (result.Success && result.Value != null)
                {
                    vm.SessionID = result.Value.Id;
                    vm.sessionID = result.Value.Id;
                    vm.LoginScreenVisible = false;
                    vm.LogsPageVisible = true;
                    vm.SettingsButtonVisible = true;
                }
                else
                {
                    vm.SessionNotExistVisible = true;
                    await Task.Delay(2000);
                    vm.SessionNotExistVisible = false;
                }
            }
        }
    
}