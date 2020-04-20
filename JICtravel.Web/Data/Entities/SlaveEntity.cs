using JICtravel.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JICtravel.Web.Data.Entities
{
    public class SlaveEntity : IdentityUser
    {   
        public string GetId()
        {
            return Id;
        }

        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public ICollection<TripEntity> Trips { get; set; }

        public decimal TotalExpensiveTrip => Trips == null ? 0 : Trips.Sum(t => t.TotalExpensives);

        public decimal NumberOfTrips => Trips == null ? 0 : Trips.Count();

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }
    }

}
