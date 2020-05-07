using DailyTask.Helper;
using DailyTask.Model;
using DailyTask.Models;
using DailyTask.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DailyTask.ViewModel
{
    public class DailyRecordsViewModel : ViewModelBase
    {
        public DailyRecordsViewModel()
        {
            initAll();
        }

        #region ICommand & Relay Command
        public ICommand iSaveRecord { get; private set; }
        public ICommand iAddRecord { get; private set; }
        #endregion

        #region members
        //private ObservableCollection<DailyRecordsModel> m_allRecord = new ObservableCollection<DailyRecordsModel>();
        private ObservableCollection<Daily> m_allRecord = new ObservableCollection<Daily>();
        private string m_origianalMD5Str = null;
        private Daily m_newAddRecord = new Daily();
        private uint m_selectedIndex = 0;
        private int m_recordCount = 0;
        //todo. use IOC here
        private DailyContext m_dbContext = new DailyContext();
        private MD5Helper m_md5 = new MD5Helper();
        #endregion

        #region Property
        public ObservableCollection<Daily> ALLRecord
        {
            get => m_allRecord;
            set
            {
                m_allRecord = value;
                NotifyPropertyChanged();
            }
        }
        public Daily NewAddRecord
        {
            get => m_newAddRecord;
            set
            {
                m_newAddRecord = value;
                NotifyPropertyChanged();
            }
        }

        public uint SelectedIndex
        {
            get => m_selectedIndex;
            set
            {
                m_selectedIndex = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Public
        public void onSelectionChange(object sender, RoutedEventArgs e)
        {
            if (m_selectedIndex > m_recordCount)
            {
                m_allRecord.Add(new Daily()
                { Date = new DateTime() }
                );
            }
        }
        #endregion

        #region Private Method
        private void initAll()
        {
            InitRelayCommands();
            var DailyList = m_dbContext.Daily.ToList();
            ALLRecord = new ObservableCollection<Daily>(DailyList);
            m_origianalMD5Str = m_md5.GenerateMD5(ALLRecord);
            m_recordCount = DailyList.Count;
            NewRecordViewModel.AddRecordEvent += OnAddRecord;
        }
        private void InitRelayCommands()
        {
            iSaveRecord = new RelayCommand(o => onSaveRecord());
            iAddRecord = new RelayCommand(o => showAddRecordWindow());

        }

        private void onSaveRecord()
        {
            if (m_origianalMD5Str != m_md5.GenerateMD5(ALLRecord))
            {
                m_dbContext.SaveChanges();
            }
            else
            {
                MessageBox.Show("noting changed", "Msg");
            }
        }
        private void showAddRecordWindow()
        {
            NewRecordView addNewRecordWindow = new NewRecordView();
            addNewRecordWindow.ShowDialog();
        }

        //todo.
        private void OnAddRecord(object sender, Daily daily)
        {
            ALLRecord.Add(daily);
            m_dbContext.Daily.Add(daily);
        }
        #endregion
    }
}
