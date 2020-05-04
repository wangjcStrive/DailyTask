using DailyTask.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DailyTask.ViewModel
{
    public class DailyRecordsViewModel : ViewModelBase
    {
        #region members
        private ObservableCollection<DailyRecordsModel> m_allRecord = new ObservableCollection<DailyRecordsModel>();

        public DailyRecordsViewModel()
        {
            ALLRecord.Add(new DailyRecordsModel("Yes", "NO", "yes"));
            ALLRecord.Add(new DailyRecordsModel("1", "2", "3"));
            ALLRecord.Add(new DailyRecordsModel("Yes", "NO", "yes"));
            ALLRecord.Add(new DailyRecordsModel("Yes", "NO", "yes"));
        }
        #endregion

        #region Property
        public ObservableCollection<DailyRecordsModel> ALLRecord
        {
            get => m_allRecord;
            set
            {
                //todo.
                //m_allRecord = value;
                //NotifyPropertyChanged(nameof(ALLRecord));
            }
        }
        #endregion
    }
}
