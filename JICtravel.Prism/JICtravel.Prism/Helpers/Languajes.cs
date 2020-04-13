using JICtravel.Prism.Interfaces;
using JICtravel.Prism.Resources;
using Xamarin.Forms;

namespace JICtravel.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Accept => Resource.Accept;

        public static string Error => Resource.Error;
        
        public static string Document => Resource.Document;

        public static string FirstName => Resource.FirstName;

        public static string LastName => Resource.LastName;
        
        public static string Email => Resource.Email;
        
        public static string CityVisited => Resource.CityVisited;
        
        public static string Expensive => Resource.Expensive;

        public static string NumberOfTrips => Resource.NumberOfTrips;

        public static string TypeExpensive => Resource.TypeExpensive;

    }

}
