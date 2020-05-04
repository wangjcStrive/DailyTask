using System;
using System.Collections.Generic;
using System.Text;

namespace DailyTask.Model
{
    public class DailyRecordsModel: ModelBase
    {
        public DailyRecordsModel(string jL, string yL, string study)
        {
            JL = jL;
            YL = yL;
            Study = study;
        }
        public string JL { get; set; }
        public string YL { get; set; }
        public string Study { get; set; }
        public bool GitHub { get; set; }
        public bool Eng { get; set; }
        public string Score { get; set; }
        //private string m_JL;
        //private string m_YL;
        //private string m_Study;

        //public string JL
        //{
        //    get => m_JL;
        //    set
        //    {
        //        m_YL = value;
        //        NotifyPropertyChanged(nameof(JL));
        //    }
        //}
        //public string YL
        //{
        //    get => m_YL;
        //    set
        //    {
        //        m_YL = value;
        //        NotifyPropertyChanged(nameof(YL));
        //    }
        //}
        //public string Study
        //{
        //    get => m_Study;
        //    set
        //    {
        //        m_YL = value;
        //        NotifyPropertyChanged(nameof(Study));
        //    }
        //}
    }
}
