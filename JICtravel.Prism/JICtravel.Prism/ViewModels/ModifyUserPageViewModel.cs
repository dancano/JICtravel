﻿using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using JICtravel.Common.Helpers;
using Xamarin.Forms;
using JICtravel.Common.Models;
using JICtravel.Prism.Views;
using JICtravel.Common.Service;

namespace JICtravel.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled;
        private ImageSource _image;
        private SlaveResponse _slave;
        private MediaFile _file;
        private readonly INavigationService _navigationService;
        private readonly IFilesHelper _filesHelper;
        private readonly IApiService _apiService;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;

        public ModifyUserPageViewModel(INavigationService navigationService, IFilesHelper filesHelper, IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _filesHelper = filesHelper;
            _apiService = apiService;
            Title = "Modify User";
            IsEnabled = true;
            Slave = JsonConvert.DeserializeObject<SlaveResponse>(Settings.User);
            Image = Slave.PictureFullPath;
        }

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePasswordAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public SlaveResponse Slave
        {
            get => _slave;
            set => SetProperty(ref _slave, value);
        }

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

        private async void SaveAsync()
        {
            var isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            var slaveRequest = new SlaveRequest
            {
                Document = Slave.Document,
                Email = Slave.Email,
                FirstName = Slave.FirstName,
                LastName = Slave.LastName,
                Password = "123456", // It doesn't matter what is sent here. It is only for the model to be valid
                PictureArray = imageArray,
                UserTypeId = 1
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.PutAsync(url, "/api", "/Account", slaveRequest, "bearer", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Settings.User = JsonConvert.SerializeObject(Slave);
            await App.Current.MainPage.DisplayAlert("Ok", "User update", "Accept");
            await NavigationService.NavigateAsync(nameof(HomePage));

        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Slave.Document))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Insert document", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Slave.FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Insert first name", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Slave.LastName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Insert last name", "Accept");
                return false;
            }

            return true;
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "Picture Source",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "From Camera")
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

        private async void ChangePasswordAsync()
        {
            await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
        }

    }

}
