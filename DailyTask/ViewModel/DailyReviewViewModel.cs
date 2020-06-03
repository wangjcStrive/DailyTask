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

            m_dbAccess.getRecordByID(ref record, m_dbAccess.getRecordCount()- DailyRecordsViewModel.daysOffset);
            string firstRecord = record.Date.ToString("yyyyMMdd") + "\n" + record.Comments +"\n\n";

            m_dbAccess.getRecordByID(ref record, m_dbAccess.getRecordCount() - DailyRecordsViewModel.daysOffset*2);
            string secondRecord = record.Date.ToString("yyyyMMdd") + "\n" + record.Comments;

            m_dailyStudy = firstRecord + secondRecord;


        }


        #region private
        private string m_dailyStudy;
        private DBAccess m_dbAccess = new DBAccess();
        private string m_recordTime;
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

        public string RecordTime
        {
            get => m_recordTime;
            set
            {
                m_recordTime = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
    }
}
