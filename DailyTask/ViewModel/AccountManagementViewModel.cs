using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using DailyTask.Models;
using DailyTask.DBHelper;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DailyTask.Helper;
using DailyTask.View;

namespace DailyTask.ViewModel
{
    public class AccountManagementViewModel : ViewModelBase
    {
        #region field
        private static NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();
        private ICollectionView m_allAccountCollectionView;
        private List<AccountRecord> m_allAccountRecord;
        private readonly DBAccess m_dbAccess = new DBAccess();
        private AccountRecord m_selectedRecord = new AccountRecord();
        private string m_contentFilter = string.Empty;
        #endregion

        public AccountManagementViewModel()
        {
            InitRelayCommands();
            updateUI();
        }

        #region RelayCommand
        public ICommand iAddRecord { get; private set; }
        public ICommand iModifyRecord { get; private set; }
        public ICommand iDelRecord { get; private set; }
        #endregion

        #region Property
        public string ContentFilter
        {
            get => m_contentFilter;
            set
            {
                m_contentFilter = value;
                NotifyPropertyChanged();
                AllAccountCollectionView.Refresh();
            }
        }
        public AccountRecord SelectedRecord
        {
            get => m_selectedRecord;
            set
            {
                m_selectedRecord = value;
                NotifyPropertyChanged();
            }
        }
        public ICollectionView AllAccountCollectionView
        {
            get => m_allAccountCollectionView;
            set
            {
                m_allAccountCollectionView = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region PrivateFunction
        private void updateUI()
        {
            m_allAccountRecord = m_dbAccess.getAllAccountRecord();
            AllAccountCollectionView = CollectionViewSource.GetDefaultView(m_allAccountRecord);
            AllAccountCollectionView.Filter = accountFilter;
            AllAccountCollectionView.Refresh();
        }
        private void InitRelayCommands()
        {
            iAddRecord = new RelayCommand(o => onAddRecordWindow());
            iModifyRecord = new RelayCommand(o => onModifyRecord());
            iDelRecord = new RelayCommand(o => onDelRecord());

        }

        private void onDelRecord()
        {
            AccountRecord recordToDel = AllAccountCollectionView.CurrentItem as AccountRecord;
            m_dbAccess.deleteRecordByType(recordToDel);
            updateUI();


        }

        private void onModifyRecord()
        {
            NewAccountRecordView newRecordView = new NewAccountRecordView(AllAccountCollectionView.CurrentItem as AccountRecord);
            newRecordView.ShowDialog();
            updateUI();
            updateUI();
        }

        private void onAddRecordWindow()
        {
            NewAccountRecordView newRecordView = new NewAccountRecordView();
            newRecordView.ShowDialog();
            updateUI();
        }

        private bool accountFilter(object obj)
        {
            AccountRecord record = obj as AccountRecord;
            if (ContentFilter == string.Empty)
                return false;

            string[] subStr = ContentFilter.Split(".");
            bool bFilterResult = false;
            if (subStr.Length >= 2 && subStr[0] == "aa")
            {
                if (subStr[1] == "all")
                    bFilterResult = true;
                else if (subStr[1] == "")   //input aa.
                    bFilterResult = false;
                else
                {
                    if (record != null && record.Comments != null)
                        bFilterResult = record.Comments.ContainsIgnoreCase(subStr[1]);
                }
            }
            return bFilterResult;
        }
        #endregion

    }
}
