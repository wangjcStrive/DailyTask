using System;
using System.Collections.Generic;
using System.Text;

namespace DailyTask.Models
{
    public class AccountRecord
    {
        public int ID { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string? Comments { get; set; }
    }
}
