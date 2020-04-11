using JICtravel.Common.Models;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JICtravel.Prism.ViewModels
{
    public class JICtravelMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public JICtravelMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_add_circle_outline",
                    PageName = "NewTripPage",
                    Title = "New trip"
                },
                new Menu
                {
                    Icon = "Airplane",
                    PageName = "HomePage",
                    Title = "My Trips"
                },
                new Menu
                {
                    Icon = "ChangeUser",
                    PageName = "ModifyUserPage",
                    Title = "Modify User"
                },
                new Menu
                {
                    Icon = "ReportIncident",
                    PageName = "ReportPage",
                    Title = "Report an incident"
                },
                new Menu
                {
                    Icon = "Exit",
                    PageName = "LoginPage",
                    Title = "Log in"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
