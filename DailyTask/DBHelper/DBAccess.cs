using DailyTask.Helper;
using DailyTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyTask.DBHelper
{
    public class DBAccess
    {
        //private HashSet<int> m_newAddRecord = new HashSet<int>();
        //private HashSet<int> m_modifiedRecord = new HashSet<int>();

        //public HashSet<int> NewAddRecordSet { get => m_newAddRecord;}
        //public HashSet<int> ModifiedRecordSet { get => m_modifiedRecord;}
        public ObservableCollection<Daily> getAllRecord()
        {
            using (var dbc = new DailyTaskContext())
            {
                //todo. asyn/await
                var DailyList = dbc.Daily.OrderByDescending(p=>p.Id);
                return new ObservableCollection<Daily>(DailyList);
                //dbc.Daily.Load();
                //allRecord = dbc.Daily.Local.ToObservableCollection();
            }
        }

        public void deleteRecord(Daily record)
        {
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    var query = dbc.Daily.Single(p => p.Id == record.Id);
                    dbc.Remove(query);
                    dbc.SaveChanges();
                }
            }
            catch (System.InvalidOperationException e)
            {
                MessageBox.Show($"can't delete record{record.Id}. {e.Message}");
            }
        }

        public void save(Daily record)
        {
            using (var dbc = new DailyTaskContext())
            {
                var query = dbc.Daily.SingleOrDefault(p => p.Id == record.Id);
                if (query != null)  //exist, update
                {
                    //todo. better soluton?
                    query.Id = record.Id;
                    query.Date = record.Date;
                    query.Week = record.Week;
                    query.Baby = record.Baby;
                    query.EarlyToBed = record.EarlyToBed;
                    query.Drink = record.Drink;
                    query.Jl = record.Jl;
                    query.EatTooMuch = record.EatTooMuch;
                    query.Washroom = record.Washroom;
                    query.Coding = record.Coding;
                    query.LearnDaily = record.LearnDaily;
                    query.Eng = record.Eng;
                    query.Efficiency = record.Efficiency;
                    query.Hz = record.Hz;
                    query.Score = record.Score;
                    query.Comments = record.Comments;
                    dbc.SaveChanges();
                    MessageBox.Show($"modify {record.Id} done!");
                }
                else            //add new
                {
                    var dateQuery = dbc.Daily.SingleOrDefault(p => p.Date == record.Date);
                    if (dateQuery == null)
                    {
                        dbc.Daily.Add(record);
                        dbc.SaveChanges();
                        MessageBox.Show($"add {record.Id} done!");
                    }
                    else
                        MessageBox.Show("date exist!");
                }
            }
        }
    }
}
