using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public string PicturePath { get; set; }

        public TripEntity Trips { get; set; }

        public ICollection<ExpensiveTypeEntity> ExpensivesType { get; set; }

    }
}
