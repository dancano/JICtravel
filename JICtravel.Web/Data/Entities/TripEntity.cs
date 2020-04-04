using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JICtravel.Web.Data.Entities
{
    public class TripEntity
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime? EndDate { get; set; }

        public DateTime? EndDateLocal => EndDate?.ToLocalTime();

        [Display(Name = "City Visited")]
        [MaxLength(100, ErrorMessage = "The {0} field must have {1} characters.")]
        public string CityVisited { get; set; }

        public decimal TotalExpensives => TripDetails == null ? 0 : TripDetails.Sum(t => t.Expensive);

        public SlaveEntity Slave { get; set; }

        public ICollection<TripDetailEntity> TripDetails { get; set; }


    }
}
