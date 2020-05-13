using DailyTask.Models;
using Microsoft.EntityFrameworkCore;
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
                var DailyList = dbc.Daily.ToList();
                return new ObservableCollection<Daily>(DailyList);
                //dbc.Daily.Load();
                //allRecord = dbc.Daily.Local.ToObservableCollection();
            }
        }

        public void addRecord(Daily dailyRecord)
        {
            using (var dbc = new DailyTaskContext())
            {
                dbc.Add(dailyRecord);
                dbc.SaveChanges();

            }
        }

        public void modifyRecord(Daily record)
        {
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    var query = dbc.Daily.Single(p => p.Id == record.Id);
                    query = record;
                    dbc.SaveChanges();
                }
            }
            catch (System.InvalidOperationException e)
            {
                MessageBox.Show($"can't find ID:{record.Id}. {e.Message}");
            }
        }

        public void deleteRecord(Daily record)
        {
            try
            {
                using(var dbc = new DailyTaskContext())
                {
                    var query = dbc.Daily.Single(p => p.Id == record.Id);
                    dbc.Remove(query);
                    dbc.SaveChanges();
                }
            }
            catch (System.InvalidOperationException e)
            { 
                MessageBox.Show($"can't delete record{record.Id}");
            }
        }

        public void save(Daily record)
        {
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    var query = dbc.Daily.SingleOrDefault(p => p.Id == record.Id);
                    if(query != null)
                    {
                        query = record;
                        dbc.SaveChanges();
                    }
                    else
                    {
                        var dateQuery = dbc.Daily.SingleOrDefault(p => p.Date == record.Date);
                        if (dateQuery == null)
                        {
                            dbc.Daily.Add(record);
                            dbc.SaveChanges();
                        }
                        else
                            MessageBox.Show("date exist!");
                    }
                }

            }
            catch (System.InvalidOperationException e)
            {
            }
        }
    }
}
