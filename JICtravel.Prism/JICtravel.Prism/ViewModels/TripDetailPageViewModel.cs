using JICtravel.Common.Models;
using JICtravel.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JICtravel.Prism.ViewModels
{
    public class TripDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private TripResponse _trip;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _newExpenseCommand;

        public TripDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Trip Detail";
            IsEnabled = true;
        }

        public DelegateCommand NewExpenseCommand => _newExpenseCommand ?? (_newExpenseCommand = new DelegateCommand(AddExpenseAsync));

        private async void AddExpenseAsync()
        {
            await _navigationService.NavigateAsync(nameof(AddExpensePage));
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

        public TripResponse Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("trip"))
            {
                Trip = parameters.GetValue<TripResponse>("trip");
            }
        }
    }


}
