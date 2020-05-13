using DailyTask.DBHelper;
using DailyTask.Helper;
using DailyTask.IOC;
using DailyTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DailyTask.ViewModel
{
    public class NewRecordViewModel : ViewModelBase
    {
        public ICommand RecordEditDone { get; private set; }
        private Daily m_dailyRecord = new Daily();
        private bool m_dialogClose;
        public static event EventHandler<EventArgs> AddRecordEvent;
        private DBAccess m_dbAccess = new DBAccess();


        #region Pubslic
        public NewRecordViewModel()
        {
            initAll();
        }
        #endregion


        #region Property
        public Daily DailyRecord
        {
            get => m_dailyRecord;
            set
            {
                m_dailyRecord = value;
                NotifyPropertyChanged();
            }
        }

        public bool DialogClose
        {
            get => m_dialogClose;
            set
            {
                m_dialogClose = value;
                NotifyPropertyChanged();
            }
        }
        #endregion


        #region private
        private void initAll()
        {
            initRelayCommand();
        }

        private void initRelayCommand()
        {
            RecordEditDone = new RelayCommand(o => onEditDone());
        }

        private void onEditDone()
        {
            // todo. score depend on other textbox. do it in xaml
            m_dailyRecord.Score = m_dailyRecord.Baby +
                m_dailyRecord.EarlyToBed +
                m_dailyRecord.Drink +
                m_dailyRecord.Jl +
                m_dailyRecord.EatTooMuch +
                m_dailyRecord.Washroom +
                m_dailyRecord.Coding +
                m_dailyRecord.LearnDaily +
                m_dailyRecord.Eng +
                m_dailyRecord.Efficiency +
                m_dailyRecord.Hz;

            m_dbAccess.save(m_dailyRecord);
            DialogClose = true;
        }
        #endregion
    }
}
