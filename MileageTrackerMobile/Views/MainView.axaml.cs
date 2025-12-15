
using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

using MileageTrackerMobile.Models;
using MileageTrackerMobile.ViewModels;

namespace MileageTrackerMobile.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        public async void Continue_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm == null) return;

            vm.SessionIdDisplayVisible = false;
            vm.HomepageVisible = false;
            vm.LogsPageVisible = true;

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
                    vm.HomepageVisible = false;
                    vm.LogsPageVisible = true;
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
                    vm.HomepageVisible = false;
                    vm.LogsPageVisible = true;
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
}
