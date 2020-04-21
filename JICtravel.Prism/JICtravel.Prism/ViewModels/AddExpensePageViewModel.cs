using JICtravel.Common.Service;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;
using System;
using JICtravel.Common.Helpers;
using JICtravel.Common.Models;
using Plugin.Media.Abstractions;
using Plugin.Media;

namespace JICtravel.Prism.ViewModels
{
    public class AddExpensePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly TripDetailRequest _tripDetail;
        private ImageSource _image;
        private bool _isRunning;
        private bool _isEnabled;
        private MediaFile _file;
        private readonly IFilesHelper _filesHelper;
        private DelegateCommand _addExpenseCommand;
        private DelegateCommand _changeImageCommand;

        public AddExpensePageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IFilesHelper filesHelper) : base(navigationService)
        {
            Title = "Add Expense";
            Image = App.Current.Resources["UrlNoImage"].ToString();
            IsEnabled = true;
            StartDate = DateTime.Today;
            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
        }

        public DelegateCommand AddExpenseCommand => _addExpenseCommand ?? (_addExpenseCommand = new DelegateCommand(AddExpenseAsync));

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

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

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public DateTime StartDate { get; set; }

        public decimal Expense { get; set; }

        public TripDetailRequest TripDetail { get; set; }

        private async void AddExpenseAsync()
        {
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                return;
            }

            SlaveResponse slave = JsonConvert.DeserializeObject<SlaveResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            TripResponse trip = JsonConvert.DeserializeObject<TripResponse>(Settings.Trip);

            TripDetailRequest tripDetailRequest = new TripDetailRequest
            {
                TripId = trip.Id,
                StartDate = StartDate,
                Expensive = Expense,
            };

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }
            tripDetailRequest.PictureArrayExpense = imageArray;

            Response response = await _apiService.NewExpenseAsync(url, "/api", "/Trips/PostExpensesTrip", tripDetailRequest, "bearer", token.Token);

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

            Settings.Trip = null;
            await _navigationService.GoBackAsync();
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "PictureSource",
                "Cancel",
                null,
                "FromGallery",
                "FromCamera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "FromCamera")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }
    }
}
