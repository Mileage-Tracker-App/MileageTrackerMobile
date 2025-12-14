using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MileageTrackerMobile;
using MileageTrackerMobile.APIController;
using MileageTrackerMobile.ViewModels;

namespace MileageTrackerMobile.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public void Login_OnCLick(object sender, RoutedEventArgs e)
    {
        MainViewModel._apiController.GetSessionAsync<TSession>(MainViewModel.LoginInput);
    }
    
}