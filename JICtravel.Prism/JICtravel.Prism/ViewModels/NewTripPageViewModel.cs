using JICtravel.Common.Helpers;
using JICtravel.Common.Models;
using JICtravel.Common.Service;
using JICtravel.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Essentials;

namespace JICtravel.Prism.ViewModels
{
    public class NewTripPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _addTripCommand;

        public NewTripPageViewModel(INavigationService navigationService, IApiService apiService) 
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "New Trip Page";
            IsEnabled = true;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        public DelegateCommand AddTripCommand => _addTripCommand ?? (_addTripCommand = new DelegateCommand(AddTripAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public String CityVisited { get; set; }

        private async void AddTripAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "You dn't have connection", "Accept");
                return;
            }

            SlaveResponse slave = JsonConvert.DeserializeObject<SlaveResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);


            TripRequest tripRequest = new TripRequest
            {
                UserId = new Guid(slave.Id),
                StartDate = StartDate,
                EndDate = EndDate,
                CityVisited = CityVisited
            };

            Response response = await _apiService.NewTripAsync(url, "/api", "/Trips", tripRequest, "bearer", token.Token);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.GoBackAsync();
        }

    }
}

