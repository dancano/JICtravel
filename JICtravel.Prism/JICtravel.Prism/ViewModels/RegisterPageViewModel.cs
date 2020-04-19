﻿using JICtravel.Common.Helpers;
using JICtravel.Common.Models;
using JICtravel.Common.Service;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace JICtravel.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private ImageSource _image;
        private readonly SlaveRequest _slave;
        private Role _role;
        private ObservableCollection<Role> _roles;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _registerCommand;

        public RegisterPageViewModel(
            INavigationService navigationService,
            IRegexHelper regexHelper,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            Title = "Register";
            Image = App.Current.Resources["UrlNoImage"].ToString();
            IsEnabled = true;
            Slave = new SlaveRequest();
            Roles = new ObservableCollection<Role>(CombosHelper.GetRoles());
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public SlaveRequest Slave 
        { 
            get; set; 
        }

        public Role Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
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

        private async void RegisterAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;
            string url = App.Current.Resources["UrlAPI"].ToString();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "You dn't have connection", "Accept");
                return;
            }

            Slave.UserTypeId = Role.Id;
            Response response = await _apiService.RegisterUserAsync(url, "/api", "/Account", Slave);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "This user already exist", "Accept");
                return;
            }

            await App.Current.MainPage.DisplayAlert("Ok", "We send a email for your confirmation", "Accept");
            await _navigationService.GoBackAsync();
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


            if (string.IsNullOrEmpty(Slave.Email) || !_regexHelper.IsValidEmail(Slave.Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Insert a email", "Accept");
                return false;
            }

            if (Role == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please, select your role", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Slave.Password) || Slave.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please insert validate password", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(Slave.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please, confirm password", "Accept");
                return false;
            }

            if (Slave.Password != Slave.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Both password are not equals", "Accept");
                return false;
            }

            return true;
        }
    }


}
