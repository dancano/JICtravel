using System;
using System.Collections.Generic;
using System.Text;

namespace JICtravel.Common.Models
{
    public class TripDetailResponse
    {

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public decimal Expensive { get; set; }

        public string PicturePath { get; set; }

        public List<ExpensiveTypeResponse> expensiveTypes { get; set; }

    }
}
