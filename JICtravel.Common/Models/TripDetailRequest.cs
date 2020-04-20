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

        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public decimal Expensive { get; set; }

        public int ExpensiveTypeId { get; set; }

        public byte[] PictureArrayExpense { get; set; }

    }
}
