using JICtravel.Common.Models;
using JICtravel.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace JICtravel.Prism.ViewModels
{
    public class TripDetailsItemViewModel : TripResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectTripCommand;

        public TripDetailsItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectTripCommand => _selectTripCommand ?? (_selectTripCommand = new DelegateCommand(SelectTripAsync));

        private async void SelectTripAsync()
        {
            await _navigationService.NavigateAsync("TripDetailPage");
            NavigationParameters parameters = new NavigationParameters
            {
                { "trip", this }
            };
            await _navigationService.NavigateAsync(nameof(TripDetailPage), parameters);
        }
    }

}
