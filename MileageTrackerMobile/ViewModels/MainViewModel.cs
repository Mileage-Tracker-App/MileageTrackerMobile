using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MileageTrackerMobile.APIController;
using MileageTrackerMobile.Models;
using MileageTrackerMobile.Modules;

namespace MileageTrackerMobile.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    [ObservableProperty] public int sessionID;
    [ObservableProperty] public List<Log> logsList;

    [ObservableProperty] public string loginInput;

    [ObservableProperty] public bool loginScreenVisible = true;
    [ObservableProperty] public bool sessionNotExistVisible = false;
    [ObservableProperty] public bool sessionIdDisplayVisible = false;
    [ObservableProperty] public bool logsPageVisible = false;
    [ObservableProperty] public bool settingsButtonVisible = false;
    [ObservableProperty] public bool settingsPageVisible = false;
    [ObservableProperty] public bool exitButtonVisible = false;
    
    [ObservableProperty] public APIController.ApiController _apiController = new APIController.ApiController();
    
    

}