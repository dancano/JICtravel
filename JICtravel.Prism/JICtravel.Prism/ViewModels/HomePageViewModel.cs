using JICtravel.Common.Models;
using JICtravel.Common.Service;
using Prism.Commands;
using Prism.Navigation;
using System.Text.RegularExpressions;

namespace JICtravel.Prism.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private readonly IApiService _apiService;
        private SlaveResponse _slave;
        private DelegateCommand _checkDocumentCommand;

        public HomePageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Trips of Slave";
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
    
        public SlaveResponse Slave
        {
            get => _slave;
            set => SetProperty(ref _slave, value);
        }

        public string Document { get; set; }

        public DelegateCommand CheckDocumentCommand => _checkDocumentCommand ?? (_checkDocumentCommand = new DelegateCommand(CheckDocumentAsync));

        private async void CheckDocumentAsync()
        {
            if (string.IsNullOrEmpty(Document))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a plaque.",
                    "Accept");
                return;
            }

            Regex regex = new Regex(@"^[0-9]*$");
            if (!regex.IsMatch(Document))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "The document must have only numbers.",
                    "Accept");
                return;
            }

            IsRunning = true;
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection.", "Accept");
                return;
            }

            Response response = await _apiService.GetTripAsync(Document, url, "api", "/Taxis");
            IsRunning = false;


            Slave = (SlaveResponse)response.Result;
        }
    }

}
