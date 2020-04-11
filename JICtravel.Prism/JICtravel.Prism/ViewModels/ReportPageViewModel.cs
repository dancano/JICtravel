using Prism.Navigation;

namespace JICtravel.Prism.ViewModels
{
    public class ReportPageViewModel : ViewModelBase
    {
        public ReportPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Report Page";
        }
    }
}
