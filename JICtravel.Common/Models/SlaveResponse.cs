using JICtravel.Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace JICtravel.Common.Models
{
    public class SlaveResponse
    {
        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public List<TripResponse> Trips { get; set; }

        public decimal TotalTrips => Trips == null ? 0 : Trips.Sum(t => t.TotalExpensives);

        public string PicturePath { get; set; }

    }
}
