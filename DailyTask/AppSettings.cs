using System;
using System.Collections.Generic;
using System.Text;

namespace DailyTask
{
    public class AppSettings
    {
        public string StringSetting { get; set; }

        public int IntegerSetting { get; set; }

        public bool BooleanSetting { get; set; }
    }

    public interface ISampleService
    {
        string GetCurrentDate();
    }

    public class SampleService : ISampleService
    {
        public string GetCurrentDate() => DateTime.Now.ToLongDateString();
    }
}
