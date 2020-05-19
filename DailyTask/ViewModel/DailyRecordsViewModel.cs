using DailyTask.DBHelper;
using DailyTask.Helper;
using DailyTask.Model;
using DailyTask.Models;
using DailyTask.View;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DailyTask.ViewModel
{
    public class DailyRecordsViewModel : ViewModelBase
    {
        public DailyRecordsViewModel()
        {
            initAll();
        }

        #region ICommand & Relay Command
        public ICommand iAddRecord { get; private set; }
        public ICommand iModifyRecord { get; private set; }
        public ICommand iDelRecord { get; private set; }
        #endregion

        #region members
        private ObservableCollection<Daily> m_allRecord = new ObservableCollection<Daily>();
        private Daily m_selectedRecord ;
        private int m_selectedIndex = 0;
        private SeriesCollection m_JLPieSeriesCollection = new SeriesCollection();
        private int m_JLCount = 0;
        private SeriesCollection m_drinkPieSeriesCollection = new SeriesCollection();
        private int m_drinkCount = 0;
        private DBAccess m_dbAccess = new DBAccess();
        private int m_monthSelectedIndex = 0;
        private List<string> m_monthList = new List<string>()
        {
            "All", "Jan","Jan","Feb","Mar","Apr","May","Jun","July","Aug","Sep","Oct","Nov","Dec"
        };
        #endregion



        #region Property
        public int MonthSelectedIndex
        {
            get => m_monthSelectedIndex;
            set
            {
                m_monthSelectedIndex = value;
                NotifyPropertyChanged();
            }
        }
        public List<string> MonthChoose
        {
            get => m_monthList;
            set
            {
                m_monthList = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<Daily> ALLRecord
        {
            get => m_allRecord;
            set
            {
                m_allRecord = value;
                NotifyPropertyChanged();
            }
        }

        public SeriesCollection JLPieSeriesCollection
        {
            get => m_JLPieSeriesCollection;
            private set { }
            //set
            //{
            //    m_JLPieSeriesCollection = value;
            //    NotifyPropertyChanged();
            //}
        }
        public SeriesCollection DrinkPieSeriesCollection
        {
            get => m_drinkPieSeriesCollection;
            private set { }
            //set
            //{
            //    m_drinkPieSeriesCollection = value;
            //    NotifyPropertyChanged();
            //}
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
                //todo. can update UI in property setter??!!
                updateUI();
            }
        }
        #endregion

        #region Private Method
        private void initAll()
        {
            updateUI();
            InitRelayCommands();
        }

        private void InitRelayCommands()
        {
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
            updateUI();
        }
        private void onModifyRecord()
        {
            NewRecordView addNewRecordWindow = new NewRecordView(SeletedRecord);
            addNewRecordWindow.ShowDialog();
            updateUI();
        }

        private void onDelRecord()
        {
            m_dbAccess.deleteRecord(SeletedRecord);
            updateUI();
        }

        private void updateDataGrid()
        {
            ALLRecord =  m_dbAccess.getAllRecord();
            ALLRecord.Reverse();
        }
        private void updateUI()
        {
            updateDataGrid();
            updatePieChardList();
        }

        private void updatePieChardList()
        {
            m_JLCount = m_allRecord.Count(p => p.Jl > 0);
            m_drinkCount = m_allRecord.Count(p => p.Drink > 0);


            m_JLPieSeriesCollection.Clear();
            m_drinkPieSeriesCollection.Clear();

            m_JLPieSeriesCollection.Add(new PieSeries { Title = "Done", Values = new ChartValues<double> { m_JLCount }, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("D({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });
            m_JLPieSeriesCollection.Add(new PieSeries { Title = "Fail", Values = new ChartValues<double> { ALLRecord.Count-m_JLCount }, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("F({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });
            

            m_drinkPieSeriesCollection.Add(new PieSeries { Title = "Done", Values = new ChartValues<double> { m_drinkCount}, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("D({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });
            m_drinkPieSeriesCollection.Add(new PieSeries { Title = "Fail", Values = new ChartValues<double> { ALLRecord.Count - m_drinkCount }, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("Fs({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });
        }
        #endregion
    }
}
