using DailyTask.Helper;
using DailyTask.IOC;
using DailyTask.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DailyTask.ViewModel
{
    public class NewRecordViewModel : ViewModelBase, IIOCService
    {
        private Daily m_dailyRecord = new Daily();
        private bool m_canAddNewRecord = false;
        private bool m_dialogClose;
        public ICommand AddRecord { get; private set; }
        public static event EventHandler<Daily> AddRecordEvent;


        #region Public
        public NewRecordViewModel()
        {
            initRelayCommand();
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
        private void initRelayCommand()
        {
            AddRecord = new RelayCommand(o => onSaveRecord());
        }

        private void onSaveRecord()
        {
            if(canSaveRecord())
            {
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
                EventHandler<Daily> ehandler = AddRecordEvent;
                if (ehandler != null)
                {
                    ehandler(this, DailyRecord);
                }

                DialogClose = true;
            }
            else
            {
                MessageBox.Show("Invalid Record", "Warning");
            }
        }
        private bool canSaveRecord()
        {
            if (m_dailyRecord.Baby == null &&
                m_dailyRecord.EarlyToBed == null &&
                m_dailyRecord.Drink == null &&
                m_dailyRecord.Jl == null &&
                m_dailyRecord.EatTooMuch == null &&
                m_dailyRecord.Washroom == null &&
                m_dailyRecord.Coding == null &&
                m_dailyRecord.LearnDaily == null &&
                m_dailyRecord.Eng == null &&
                m_dailyRecord.Efficiency == null &&
                m_dailyRecord.Hz == null
                )
                return false;
            else
                return true;
        }

        public string test()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
