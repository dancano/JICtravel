using System;
using System.Collections.Generic;

namespace JICtravel.Common.Models
{
    public class TripDetailResponse
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public decimal Expensive { get; set; }

        public string PicturePathExpense { get; set; }

        public string PictureFullPathExpense => string.IsNullOrEmpty(PicturePathExpense)
        ? "https://jictravelweb.azurewebsites.net//images/noimage.png"
        : $"https://jictravelweb.azurewebsites.net{PicturePathExpense.Substring(1)}";

        public ExpensiveTypeResponse expensiveTypes { get; set; }

    }
}
