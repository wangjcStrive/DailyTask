using DailyTask.DBHelper;
using DailyTask.Helper;
using DailyTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DailyTask.ViewModel
{
    public class NewAccountRecordViewModel : ViewModelBase
    {
        public NewAccountRecordViewModel()
        {
            InitRelayCommands();
        }
        public NewAccountRecordViewModel(AccountRecord accountRecordToModify)
        {
            AccountRecordToModify = accountRecordToModify;
            InitRelayCommands();
        }

        #region Field
        private AccountRecord m_accountRecordtoModify = new AccountRecord();
        private readonly DBAccess m_dbAccess = new DBAccess();
        private bool m_dialogClose;
        #endregion


        #region RelayCommand
        public ICommand iRecordModifyDone { get; private set; }
        #endregion

        #region Property
        public AccountRecord AccountRecordToModify
        {
            get => m_accountRecordtoModify;
            set
            {
                m_accountRecordtoModify = value;
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


        #region PrivateMethod
        private void InitRelayCommands()
        {
            iRecordModifyDone = new RelayCommand(p => onRecordModifyDone());
        }

        private void onRecordModifyDone()
        {
            //if (AccountRecordToModify.AccountName != null && AccountRecordToModify.Password != null)
            if (!string.IsNullOrWhiteSpace(AccountRecordToModify.AccountName) && !string.IsNullOrWhiteSpace(AccountRecordToModify.Password) && !string.IsNullOrWhiteSpace(AccountRecordToModify.Comments))
            {
                m_dbAccess.addAccountRecord(AccountRecordToModify);
                DialogClose = true;
            }
            else
            {
                MessageBox.Show($"account & password can't be null!");
            }
        }
        #endregion
    }
}
