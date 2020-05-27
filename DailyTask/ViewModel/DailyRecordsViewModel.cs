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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DailyTask.ViewModel
{
    public class DailyRecordsViewModel : ViewModelBase
    {
        public static int daysOffset = 12;
        const string TimeToReview = "05:27";
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
        private Daily m_selectedRecord;
        private int m_selectedIndex = 0;
        private SeriesCollection m_JLPieSeriesCollection = new SeriesCollection();
        private int m_JLDoneCount = 0;
        private int m_JLFailCount = 0;
        private SeriesCollection m_drinkPieSeriesCollection = new SeriesCollection();
        private int m_drinkDoneCount = 0;
        private int m_drinkFailCount = 0;
        private SeriesCollection m_totalScoreSeriesCollection = new SeriesCollection();

        private DBAccess m_dbAccess = new DBAccess();
        private int m_monthSelectedIndex = 0;
        private List<string> m_monthList = new List<string>()
        {
            "All", "Jan","Feb","Mar","Apr","May","Jun","July","Aug","Sep","Oct","Nov","Dec"
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
                //todo. can update UI in property setter??!!
                updatePieChardList();
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
        }
        public SeriesCollection DrinkPieSeriesCollection
        {
            get => m_drinkPieSeriesCollection;
            private set { }
        }
        public SeriesCollection TotalScoreSeriesCollection
        {
            get => m_totalScoreSeriesCollection;
            private set { }
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
        public async void showReview()
        {
            Task t = Task.Run(() =>
            {
                while (true)
                {
                    string currentTime = DateTime.Now.ToString("hh:mm");
                    if (currentTime == TimeToReview)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                               {
                                   Daily record = new Daily();
                                   m_dbAccess.getRecordByID(ref record, m_allRecord.Count - daysOffset);
                                   if (record.Comments != null)
                                   {
                                       var result = MessageBox.Show("time to review", "review", MessageBoxButton.YesNo);
                                       if (result == MessageBoxResult.Yes)
                                       {
                                           DailyReviewView recordRevie = new DailyReviewView();
                                           recordRevie.ShowDialog();
                                       }
                                   }
                                   else
                                       MessageBox.Show($"nothing to review on {record.Date.ToString("yyyyMMdd")}");
                               })
                            );
                        break;
                    }
                    Thread.Sleep(10000);
                }

            });
            await t;
        }

        private void initAll()
        {
            updateUI();
            InitRelayCommands();
            //showReview();
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
                Id = ALLRecord.Count + 1,
                Baby = 0,
                Coding = 0,
                Date = DateTime.Now,
                //Week = DateTime.Now.DayOfWeek.ToString(),
                Drink = 0,
                EarlyToBed = 0,
                EatTooMuch = 0,
                Efficiency = 0,
                Eng = 0,
                Hz = 0,
                Washroom = 0,
                LearnDaily = 0,
                Jl = 0,
                Reviewd = 0,
                Comments=""
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
            ALLRecord = new ObservableCollection<Daily>(m_dbAccess.getAllRecord().OrderByDescending(p => p.Date));
        }
        private void updateUI()
        {
            updateDataGrid();
            updatePieChardList();
        }

        private void updatePieChardList()
        {
            m_JLDoneCount = m_allRecord.Count(p => p.Jl > 0 && (m_monthSelectedIndex == 0 ? true : long.Parse(p.Date.ToString("yyyyMM")) == (202000 + m_monthSelectedIndex)));
            m_JLFailCount = m_allRecord.Count(p => p.Jl == 0 && (m_monthSelectedIndex == 0 ? true : long.Parse(p.Date.ToString("yyyyMM")) == (202000 + m_monthSelectedIndex)));
            m_drinkDoneCount = m_allRecord.Count(p => p.Drink > 0 && (m_monthSelectedIndex == 0 ? true : long.Parse(p.Date.ToString("yyyyMM")) == (202000 + m_monthSelectedIndex)));
            m_drinkFailCount = m_allRecord.Count(p => p.Drink == 0 && (m_monthSelectedIndex == 0 ? true : long.Parse(p.Date.ToString("yyyyMM")) == (202000 + m_monthSelectedIndex)));


            m_JLPieSeriesCollection.Clear();
            m_drinkPieSeriesCollection.Clear();
            m_totalScoreSeriesCollection.Clear();

            m_JLPieSeriesCollection.Add(new PieSeries { Title = "Done", Values = new ChartValues<double> { m_JLDoneCount }, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("D ({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });
            m_JLPieSeriesCollection.Add(new PieSeries { Title = "Fail", Values = new ChartValues<double> { m_JLFailCount }, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("F ({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });

            m_drinkPieSeriesCollection.Add(new PieSeries { Title = "Done", Values = new ChartValues<double> { m_drinkDoneCount }, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("D ({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });
            m_drinkPieSeriesCollection.Add(new PieSeries { Title = "Fail", Values = new ChartValues<double> { m_drinkFailCount }, DataLabels = true, LabelPoint = (chartPoint) => { return string.Format("F ({0} {1:p0})", chartPoint.Y, chartPoint.Participation); } });

            if (m_monthSelectedIndex == 0 )
            {
                List<double> monthScore = m_allRecord.OrderBy(p=>p.Id).Select(p => p.Score ?? default(double)).ToList();
                m_totalScoreSeriesCollection.Add(new LineSeries { Title = m_monthSelectedIndex.ToString(), Values = new ChartValues<double>(monthScore) });
                //for (int i=0; i<DateTime.Now.Month; i++)
                //{
                //    List<double> monthScore = m_allRecord.Where(p => p.Date.Month == i).Select(p => p.Score??default(double)).ToList();
                //    m_totalScoreSeriesCollection.Add(new LineSeries { Title = i.ToString(), Values = new ChartValues<double> (monthScore) });
                //}
            }
            else
            {
                List<double> monthScore = m_allRecord.OrderBy(p => p.Id).Where(p => p.Date.Month == m_monthSelectedIndex).Select(p => p.Score ?? default(double)).ToList();
                m_totalScoreSeriesCollection.Add(new LineSeries { Title = m_monthSelectedIndex.ToString(), Values = new ChartValues<double>(monthScore) });
            }
        }
        #endregion
    }
}
