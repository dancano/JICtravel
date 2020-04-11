using Prism.Navigation;

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
