using DailyTask.DBHelper;
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
using System.Threading.Tasks;
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

        public void SaveRecord()
        {
            //if (isRecordModified())
            //{
            //    m_dbAccess.save();
            //    MessageBox.Show($"{m_dbAccess.NewAddRecordSet.Count} add! {m_dbAccess.ModifiedRecordSet.Count} modified!");
            //}
        }

        #region ICommand & Relay Command
        public ICommand iSaveRecord { get; private set; }
        public ICommand iAddRecord { get; private set; }
        public ICommand iModifyRecord { get; private set; }
        public ICommand iDelRecord { get; private set; }
        #endregion

        #region members
        //private ObservableCollection<DailyRecordsModel> m_allRecord = new ObservableCollection<DailyRecordsModel>();
        private ObservableCollection<Daily> m_allRecord = new ObservableCollection<Daily>();
        private Daily m_selectedRecord ;
        private int m_selectedIndex = 0;
        //todo. use IOC here
        private DailyTaskContext m_dbContext = new DailyTaskContext();
        private MD5Helper m_md5 = new MD5Helper();
        private DBAccess m_dbAccess = new DBAccess();
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
        public Daily SeletedRecord
        {
            get => m_selectedRecord;
            set
            {
                m_selectedRecord = value;
                NotifyPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get => m_selectedIndex;
            set
            {
                m_selectedIndex = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Private Method
        private void initAll()
        {
            InitRelayCommands();
            //todo. are ther better solution for update parent window when close derived window
            //NewRecordViewModel.AddRecordEvent += (object s, EventArgs e) => { updateDataGrid(); };
            updateDataGrid();
        }

        private void InitRelayCommands()
        {
            iSaveRecord = new RelayCommand(o => SaveRecord());
            iAddRecord = new RelayCommand(o => onAddRecordWindow());
            iModifyRecord = new RelayCommand(o => onModifyRecord());
            iDelRecord = new RelayCommand(o => onDelRecord());

        }
        private void onAddRecordWindow()
        {
            NewRecordView addNewRecordWindow = new NewRecordView(new Daily()
                {
                    Id = ALLRecord.Count+1,
                    Baby = 0,
                    Coding = 0,
                    Date = DateTime.Now,
                    Week = DateTime.Now.DayOfWeek.ToString(),
                    Drink = 0,
                    EarlyToBed = 0,
                    EatTooMuch = 0,
                    Efficiency = 0,
                    Eng = 0,
                    Hz = 0,
                    Washroom = 0,
                    LearnDaily = 0,
                    Jl = 0,
                }
            );
            addNewRecordWindow.ShowDialog();
            updateDataGrid();
        }
        private void onModifyRecord()
        {
            NewRecordView addNewRecordWindow = new NewRecordView(SeletedRecord);
            addNewRecordWindow.ShowDialog();
            updateDataGrid();
        }

        private void onDelRecord()
        {
            m_dbAccess.deleteRecord(SeletedRecord);
            updateDataGrid();
        }

        private void updateDataGrid()
        {
            ALLRecord =  m_dbAccess.getAllRecord();
        }
        #endregion
    }
}
