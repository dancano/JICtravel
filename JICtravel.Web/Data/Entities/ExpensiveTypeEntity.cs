using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JICtravel.Web.Data.Entities
{
    public class ExpensiveTypeEntity
    {
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "The {0} field must have {1} characters.")]
        [Display(Name = "Expensive Type")]
        public string ExpensiveType { get; set; }

        public ICollection<TripDetailEntity> TripDetails { get; set; }

    }
}
