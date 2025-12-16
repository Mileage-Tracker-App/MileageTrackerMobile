using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MileageTrackerMobile.Models;
using MileageTrackerMobile.ViewModels;

namespace MileageTrackerMobile.Modules;

public partial class LogsList : UserControl
{
    public LogsList()
    {
        InitializeComponent();
        UpdateLogsList();
    }
    public async void UpdateLogsList()
    {
        var vm = DataContext as MainViewModel;

        var result = await vm._apiController.GetSessionAsync<TSession>(vm.SessionID);
            
        if (result.Success && result.Value != null)
        {
            LogsListitem.ItemsSource = result.Value.Logs;
            vm.LogsList = result.Value.Logs;
        }
    }
}