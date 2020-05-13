using System;
using System.Collections.Generic;

namespace DailyTask.Models
{
    public partial class Daily
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Week { get; set; }
        public double? Baby { get; set; }
        public double? EarlyToBed { get; set; }
        public double? Drink { get; set; }
        public double? Jl { get; set; }
        public double? EatTooMuch { get; set; }
        public double? Washroom { get; set; }
        public double? Coding { get; set; }
        public double? LearnDaily { get; set; }
        public double? Eng { get; set; }
        public double? Efficiency { get; set; }
        public double? Hz { get; set; }
        public double? Score { get; set; }
        public string Comments { get; set; }
    }
}
