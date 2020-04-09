using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JICtravel.Prism.ViewModels
{
    public class ReportIncidentPageViewModel : ViewModelBase
    {
        public ReportIncidentPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Report Incident";
        }
    }
}
