
using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MileageTrackerMobile.Models;
using MileageTrackerMobile.ViewModels;
using MileageTrackerMobile.Modules;

namespace MileageTrackerMobile.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            UpdateLogsList();
        }
        
        private async void UpdateLogsList()
        {
            var vm = DataContext as MainViewModel;

            var result = await vm._apiController.GetSessionAsync<TSession>(vm.SessionID);
            
            if (result.Success && result.Value != null)
            {
                vm.LogsList = result.Value.Logs;
            }
        }
        
        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
