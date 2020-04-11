using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JICtravel.Prism.ViewModels
{
    public class NewTripPageViewModel : ViewModelBase
    {
        public NewTripPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "New Trip Page";
        }
    }
}
