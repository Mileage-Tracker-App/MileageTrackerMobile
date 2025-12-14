using CommunityToolkit.Mvvm.ComponentModel;
using MileageTrackerMobile.APIController;

namespace MileageTrackerMobile.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    [ObservableProperty] public int sessionID;

    [ObservableProperty] public static string loginInputstr;
    public static int LoginInput = int.Parse(loginInputstr);

    [ObservableProperty] public bool homepageVisible = true;
    
    [ObservableProperty] public static APIController.ApiController _apiController = new APIController.ApiController();
    
    

}