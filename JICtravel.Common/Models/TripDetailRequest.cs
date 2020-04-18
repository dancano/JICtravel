using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JICtravel.Common.Models
{
    public class TripDetailRequest
    {
        [Required]
        public int TripId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public decimal Expensive { get; set; }

        public decimal ExpensiveTypeId { get; set; }

        [Display(Name = "Invoice Picture")]
        public string PicturePath { get; set; }

    }
}
