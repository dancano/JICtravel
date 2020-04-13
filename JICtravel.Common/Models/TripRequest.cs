using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace JICtravel.Common.Models
{
    public class TripRequest
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public Guid UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public DateTime? EndDate { get; set; }

        public DateTime? EndDateLocal => EndDate?.ToLocalTime();

        public string CityVisited { get; set; }

    }

}
