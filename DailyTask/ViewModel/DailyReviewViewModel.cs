using DailyTask.DBHelper;
using DailyTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace DailyTask.ViewModel
{
    public class DailyReviewViewModel : ViewModelBase
    {
        public DailyReviewViewModel()
        {
            Daily record = new Daily();
            m_dbAccess.getRecordMonthAgo(ref record);
            m_dailyStudy = record.Comments;
        }


        #region private
        private string m_dailyStudy;
        private DBAccess m_dbAccess = new DBAccess();

        #endregion


        #region Property
        public string DailyStudey
        {
            get => m_dailyStudy;
            set
            {
                m_dailyStudy = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
    }
}
