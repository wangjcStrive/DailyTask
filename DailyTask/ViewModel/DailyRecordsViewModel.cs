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
using Microsoft.Extensions.DependencyInjection;
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
using NLog.Extensions.Logging;
using System.Net.Http.Headers;
using System.ComponentModel;
using System.Windows.Data;
using DailyTask.Module;

namespace DailyTask.ViewModel
{
    public class DailyRecordsViewModel : ViewModelBase
    {
        public static int daysOffset = 7;
        const string TimeToReview = "04:30";
        public DailyRecordsViewModel()
        {
            m_logger.Info("*************** Main Window start! ***************");
            initAll();
        }

        #region ICommand & Relay Command
        public ICommand iAddRecord { get; private set; }
        public ICommand iModifyRecord { get; private set; }
        public ICommand iDelRecord { get; private set; }
        public ICommand iRecordReview { get; private set; }
        #endregion

        #region members
        private DBAccess m_dbAccess = new DBAccess();
        private ICollectionView m_allRecordCollectionView;
        private List<Daily> m_allRecord;

        private SeriesCollection m_JLPieSeriesCollection = new SeriesCollection();
        private int m_JLDoneCount = 0;
        private int m_JLFailCount = 0;
        private SeriesCollection m_drinkPieSeriesCollection = new SeriesCollection();
        private int m_drinkDoneCount = 0;
        private int m_drinkFailCount = 0;
        private SeriesCollection m_totalScoreSeriesCollection = new SeriesCollection();

        private int m_monthSelectedIndex = DateTime.Now.Month;
        private List<string> m_monthList = new List<string>()
        {
            "All", "Jan","Feb","Mar","Apr","May","Jun","July","Aug","Sep","Oct","Nov","Dec"
        };
        private static NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();
        private string m_longestDrink = string.Empty;
        private string m_longestJL = string.Empty;
        private string m_commentsFilter = string.Empty;
        private string m_weatherInfo = string.Empty;
        #endregion



        #region Property
        public string WeatherInfo
        {
            get => m_weatherInfo;
            set
            {
                m_weatherInfo = value;
                NotifyPropertyChanged();
            }
        }
        public string CommentsFilter
        {
            get => m_commentsFilter;
            set
            {
                m_commentsFilter = value;
                NotifyPropertyChanged();
                AllRecordCollectionView.Refresh();
            }
        }
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
        public ICollectionView AllRecordCollectionView
        {
            get => m_allRecordCollectionView;
            set
            {
                m_allRecordCollectionView = value;
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

        public string LongestJL
        {
            get => m_longestJL;
            set
            {
                m_longestJL = value;
                NotifyPropertyChanged();
            }
        }

        public string LongestDrink
        {
            get => m_longestDrink;
            set
            {
                m_longestDrink = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Private Method
        public async void showReview()
        {
            Task t = Task.Run(() =>
            {
                string date = string.Empty;
                while (true)
                {
                    string currentTime = DateTime.Now.ToString("hh:mm");
                    if (currentTime == TimeToReview && date != DateTime.Now.ToShortDateString())
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                               {
                                   //List<int> recordToUpdate;
                                   //DailyReviewView recordRevie = new DailyReviewView(getReviewStr(out recordToUpdate));
                                   //recordRevie.ShowDialog();
                                   //m_dbAccess.updateReiveStatus(recordToUpdate);
                                   onReviewRecord();
                               })
                            );
                        date = DateTime.Now.ToShortDateString();
                        break;
                    }
                    Thread.Sleep(10000);
                }

            });
            await t;
            updateUI();
        }

        private async void getWeather()
        {
            await Task.Run(() =>
            {
                WeatherInfo = Weather.getWeather();
            });
        }
        private void initAll()
        {
            getWeather();
            updateUI();
            InitRelayCommands();
            showReview();
        }

        private void InitRelayCommands()
        {
            iAddRecord = new RelayCommand(o => onAddRecordWindow());
            iModifyRecord = new RelayCommand(o => onModifyRecord());
            iDelRecord = new RelayCommand(o => onDelRecord());
            iRecordReview = new RelayCommand(o => onReviewRecord());

        }
        private void onAddRecordWindow()
        {
            NewRecordView addNewRecordWindow = new NewRecordView(new Daily()
            {
                Id = m_allRecord.Count + 1,
                Baby = 0,
                Coding = 0,
                Date = DateTime.Now,
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
                Comments = string.Empty
            }
            );
            addNewRecordWindow.ShowDialog();
            updateUI();
        }
        private void onModifyRecord()
        {
            NewRecordView addNewRecordWindow = new NewRecordView(AllRecordCollectionView.CurrentItem as Daily);
            addNewRecordWindow.ShowDialog();
            updateUI();
        }

        private void onDelRecord()
        {
            Daily selectedRecord = AllRecordCollectionView.CurrentItem as Daily;
            m_dbAccess.deleteRecordByID(selectedRecord.Id);
            updateUI();
        }

        //主动review之前的
        private void onReviewRecord()
        {
            var recordToReview = AllRecordCollectionView.CurrentItem as Daily;
            List<int> recordToUpdate;
            DailyReviewView recordRevie = new DailyReviewView(recordToReview.Comments + "\n\n" + getReviewStr(out recordToUpdate));
            recordRevie.ShowDialog();
            recordToUpdate.Add(recordToReview.Id);
            m_dbAccess.updateReiveStatus(recordToUpdate);
            updateUI();
        }
        private string getReviewStr(out List<int> recordID)
        {
            string reviewStr = string.Empty;
            recordID = new List<int>();


            //todo. better solution? not sure allRecordWithRiviewInfo.Count
            if (m_allRecord.Count > 2)
            {
                reviewStr += m_allRecord[2].Date.ToString("yyyyMMdd") + "\n" + m_allRecord[2].Comments + "\n\n";
                recordID.Add(m_allRecord[2].Id);
            }
            if (m_allRecord.Count > 6)
            {
                reviewStr += m_allRecord[6].Date.ToString("yyyyMMdd") + "\n" + m_allRecord[6].Comments + "\n\n";
                recordID.Add(m_allRecord[6].Id);
            }
            if (m_allRecord.Count > 13)
            {
                reviewStr += m_allRecord[13].Date.ToString("yyyyMMdd") + "\n" + m_allRecord[13].Comments + "\n\n";
                recordID.Add(m_allRecord[13].Id);
            }
            if (m_allRecord.Count > 30)
            {
                reviewStr += m_allRecord[30].Date.ToString("yyyyMMdd") + "\n" + m_allRecord[30].Comments + "\n\n";
                recordID.Add(m_allRecord[30].Id);
            }

            return reviewStr;
        }

        private void updateUI()
        {
            updateDataGrid();
            updatePieChardList();
            updateLongestRecord();
        }

        private void updateLongestRecord()
        {
            uint jl_done = 0, jl_fail = 0, max_jl_done = 0, max_jl_fail = 0, drink_done = 0, drink_fail = 0, max_drink_done = 0, max_drink_fail = 0;
            foreach (var item in m_allRecord)
            {
                if (item.Jl == 1)
                {
                    jl_done++;
                    jl_fail = 0;

                }
                else
                {
                    jl_fail++;
                    jl_done = 0;
                }

                if (item.Drink == 0)
                {
                    drink_fail++;
                    drink_done = 0;
                }
                else
                {
                    drink_done++;
                    drink_fail = 0;
                }

                if (jl_fail > max_jl_fail)
                    max_jl_fail = jl_fail;
                if (jl_done > max_jl_done)
                    max_jl_done = jl_done;
                if (drink_fail > max_drink_fail)
                    max_drink_fail = drink_fail;
                if (drink_done > max_drink_done)
                    max_drink_done = drink_done;
            }
            LongestJL = $" JL Fail {max_jl_fail}   Done {max_jl_done}";
            LongestDrink = $" Dr Fail {max_drink_fail}   Done {max_drink_done}";
        }

        private void updateDataGrid()
        {
            m_allRecord = m_dbAccess.getAllRecord();
            //m_allRecord = await m_dbAccess.getAllRecordAsync();
            //todo!! 可以不用ObservableCollection吗？这里要更新datagrid，每次都要重新GetDefaultView()，有没有其他方法。
            AllRecordCollectionView = CollectionViewSource.GetDefaultView(new ObservableCollection<Daily>(m_allRecord));
            AllRecordCollectionView.Filter = commentsFilter;
            AllRecordCollectionView.Refresh();
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

            if (m_monthSelectedIndex == 0)
            {
                List<double> monthScore = m_allRecord.OrderBy(p => p.Id).Select(p => p.Score ?? default(double)).ToList();
                m_totalScoreSeriesCollection.Add(new LineSeries { Title = m_monthSelectedIndex.ToString(), Values = new ChartValues<double>(monthScore) });
            }
            else
            {
                List<double> monthScore = m_allRecord.OrderBy(p => p.Id).Where(p => p.Date.Month == m_monthSelectedIndex).Select(p => p.Score ?? default(double)).ToList();
                m_totalScoreSeriesCollection.Add(new LineSeries { Title = m_monthSelectedIndex.ToString(), Values = new ChartValues<double>(monthScore) });
            }
        }

        private bool commentsFilter(object obj)
        {
            Daily record = obj as Daily;
            if (CommentsFilter == string.Empty)
                return true;
            bool bFilterResult = false;
            if (record != null && record.Comments != null)
                bFilterResult = record.Comments.ContainsIgnoreCase(CommentsFilter);
            return bFilterResult;
        }
        #endregion
    }
}
