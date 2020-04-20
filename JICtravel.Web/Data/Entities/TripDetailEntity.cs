using System;
using System.ComponentModel.DataAnnotations;

namespace JICtravel.Web.Data.Entities
{
    public class TripDetailEntity
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public decimal Expensive { get; set; }

        [Display(Name = "Invoice Picture")]
        public string PicturePathExpense { get; set; }

        public TripEntity Trips { get; set; }

        public ExpensiveTypeEntity ExpensiveType { get; set; }

    }
}
