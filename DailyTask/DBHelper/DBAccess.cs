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
        private static NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();
        public List<Daily> getAllRecord()
        {
            using (var dbc = new DailyTaskContext())
            {
                //todo. asyn/await
                return dbc.Daily.OrderByDescending(p=>p.Id).ToList();
            }
        }

        public int getRecordCount()
        {
            int recordCount = 0;
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    recordCount = dbc.Daily.Count();
                }
            }
            catch (System.InvalidOperationException e)
            {
                m_logger.Error("can't get record count!");
                MessageBox.Show($"can't get record count! {e.Message}");
            }

            return recordCount;
        }

        public void getRecordByID(ref Daily record, int id)
        {
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    record = dbc.Daily.Single(p => p.Id == id);
                }
            }
            catch (System.InvalidOperationException e)
            {
                m_logger.Error($"can't get record ID:{id}! {e.Message}");
                MessageBox.Show($"can't get record ID:{id}. {e.Message}");
            }
        }

        public void updateReiveStatus(List<int> IDList)
        {
            int recordIndex = 0;
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    foreach (var item in IDList)
                    {
                        recordIndex++;
                        var query = dbc.Daily.Single(p => p.Id == item);
                        if (query.Reviewd == null)
                            query.Reviewd = 1;
                        else
                            query.Reviewd = query.Reviewd + 1;
                        dbc.SaveChanges();
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                m_logger.Error($"can't update record {IDList[recordIndex]}. review status. {e.Message}");
                MessageBox.Show($"can't update record {IDList[recordIndex]}. review status. {e.Message}");
            }
        }

        public void deleteRecordByID(int recordID)
        {
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    var query = dbc.Daily.Single(p => p.Id == recordID);
                    dbc.Remove(query);
                    dbc.SaveChanges();
                }
            }
            catch (System.InvalidOperationException e)
            {
                m_logger.Error($"can't delete record{recordID}. {e.Message}");
                MessageBox.Show($"can't delete record{recordID}. {e.Message}");
            }
        }

        public void save(Daily record)
        {
            try
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
                        query.Reviewd = record.Reviewd;
                        dbc.SaveChanges();
                        m_logger.Info($"modify {record.Id} {record.Week} {record.Date:yyyyMMdd} done!");
                        MessageBox.Show($"modify {record.Id} done!");
                    }
                    else            //add new
                    {
                        var dateQuery = dbc.Daily.SingleOrDefault(p => p.Date == record.Date);
                        if (dateQuery == null)
                        {
                            dbc.Daily.Add(record);
                            dbc.SaveChanges();
                            m_logger.Info($"add {record.Id} {record.Week} {record.Date:yyyyMMdd} done!");
                            MessageBox.Show($"add {record.Id} done!");
                        }
                        else
                            MessageBox.Show("date exist!");
                    }
                }
            }
            catch (Exception e)
            {
                m_logger.Error($"failt to save record {record.Id}\n. {e.InnerException.Message}");
                MessageBox.Show($"failt to save record {record.Id}\n. {e.InnerException.Message}");
            }

        }
    }
}
