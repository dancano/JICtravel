using JICtravel.Common.Helpers;
using JICtravel.Common.Models;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JICtravel.Prism.ViewModels
{
    public class JICtravelMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private SlaveResponse _user;

        public JICtravelMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadUser();
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public SlaveResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        
        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<SlaveResponse>(Settings.User);
            }
        }

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
