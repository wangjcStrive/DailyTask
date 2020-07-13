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
                return dbc.Daily.OrderByDescending(p=>p.Date).ToList();
            }
        }

        public List<AccountRecord> getAllAccountRecord()
        {
            using (var dbc = new DailyTaskContext())
            {
                return dbc.accountRecords.ToList();
            }
        }

        public void addAccountRecord(AccountRecord newRecord)
        {
            try
            {
                using (var dbc = new DailyTaskContext())
                {
                    dbc.accountRecords.Add(newRecord);
                    dbc.SaveChanges();
                }
            }
            catch (Exception e)
            {
                m_logger.Error($"save account record {newRecord.ID} faile! {e.Message}");
                MessageBox.Show($"save account record {newRecord.ID} faile! {e.Message}");
            }
        }

        //todo. 还是不能使用异步，因为后面pie chart及其他部分都需要这里从数据库读到的allRecord
        public async Task<List<Daily>> getAllRecordAsync()
        {
            List<Daily> allRecord = new List<Daily>();
            await Task.Run(() =>
            {
                using (var dbc = new DailyTaskContext())
                {
                    allRecord =  dbc.Daily.OrderByDescending(p=>p.Date).ToList();
                }
            });
            return allRecord;
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

        public void deleteRecordByType(object record)
        {
            try
            {
                using (var dbc = new DailyTaskContext())
                {

                    if ("AccountRecord" == record.GetType().Name)
                    {
                        var accountRecord = record as AccountRecord;
                        var query = dbc.accountRecords.Single(p => p.ID == accountRecord.ID);
                        dbc.Remove(query);
                    }
                    else if ("Daily" == record.GetType().Name)
                    {
                        var dailyRecord = record as Daily;
                        var query = dbc.Daily.Single(p => p.Id == dailyRecord.Id);
                        dbc.Remove(query);

                    }
                    else
                    {
                        m_logger.Error($"invalid record type {record.GetType().Name}!");
                        MessageBox.Show($"invalid record type {record.GetType().Name}!");
                        throw new Exception("invalid record type!");
                    }
                    dbc.SaveChanges();
                }

            }
            catch (Exception)
            {

                throw;
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
                        //todo. better soluton? 遍历foreach所有的property
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
                        m_logger.Info($"modify {record.Id} {record.Week} {record.Date:yyyy/MM/dd} done!");
                        MessageBox.Show($"modify {record.Id}, {record.Date:yyyy/MM/dd}! score: {record.Score}");
                    }
                    else            //add new
                    {
                        var dateQuery = dbc.Daily.SingleOrDefault(p => p.Date == record.Date);
                        if (dateQuery == null)
                        {
                            dbc.Daily.Add(record);
                            dbc.SaveChanges();
                            m_logger.Info($"add {record.Id} {record.Week} {record.Date:yyyy/MM/dd} done!");
                            MessageBox.Show($"add {record.Id}, {record.Date:yyyy/MM/dd}! score: {record.Score}");
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
