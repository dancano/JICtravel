using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace JICtravel.Web.Data.Entities
{
    public class ExpensiveTypeEntity
    {
        public int Id { get; set; }

        [Display(Name = "Expensive Type")]
        public string ExpensiveType { get; set; }

        public TripDetailEntity TripDetails { get; set; }

    }
}
