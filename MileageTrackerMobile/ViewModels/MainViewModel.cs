using CommunityToolkit.Mvvm.ComponentModel;
using MileageTrackerMobile.APIController;

namespace MileageTrackerMobile.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    [ObservableProperty] public int sessionID;

    [ObservableProperty] public string loginInput;

    [ObservableProperty] public bool homepageVisible = true;
    [ObservableProperty] public bool sessionNotExistVisible = false;
    
    [ObservableProperty] public APIController.ApiController _apiController = new APIController.ApiController();
    
    

}