using System;
using System.Collections.Generic;
using System.Linq;

namespace JICtravel.Common.Models
{
    public class TripResponse
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public DateTime? EndDate { get; set; }

        public DateTime? EndDateLocal => EndDate?.ToLocalTime();

        public string CityVisited { get; set; }

        public List<TripDetailResponse> tripDetails { get; set; }

        public SlaveResponse User { get; set; }

        public decimal TotalExpensives => tripDetails == null ? 0 : tripDetails.Sum(t => t.Expensive);

    }
}
