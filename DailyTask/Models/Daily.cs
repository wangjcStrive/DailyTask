using DailyTask.Model;
using System;
using System.Collections.Generic;

namespace DailyTask.Models
{
    public partial class Daily : ModelBase
    {
        private DateTime? m_date;
        private string? m_week;
        private double? m_baby;
        private double? m_earlyToBed;
        private double? m_drink;
        private double? m_jl;
        private double? m_eatTooMuch;
        private double? m_washRoom;
        private double? m_coding;
        private double? m_learnDaily;
        private double? m_eng;
        private double? m_efficiency;
        private double? m_hz;
        private double? m_score;



        public DateTime? Date
        {
            get => m_date;
            set
            {
                m_date = value;
                NotifyPropertyChanged();
            }
        }
        public string Week 
        {
            get => m_week;
            set
            {
                m_week = value;
                NotifyPropertyChanged();
            }
        }
        public double? Baby
        {
            get => m_baby;
            set
            {
                m_baby = value;
                NotifyPropertyChanged();
            }
        }
        public double? EarlyToBed
        {
            get => m_earlyToBed;
            set
            {
                m_earlyToBed = value;
                NotifyPropertyChanged();
            }
        }
        public double? Drink
        {
            get => m_drink;
            set
            {
                m_drink = value;
                NotifyPropertyChanged();
            }
        }
        public double? Jl
        {
            get => m_jl;
            set
            {
                m_jl = value;
                NotifyPropertyChanged();
            }
        }
        public double? EatTooMuch
        {
            get => m_eatTooMuch;
            set
            {
                m_eatTooMuch = value;
                NotifyPropertyChanged();
            }
        }
        public double? Washroom
        {
            get => m_washRoom;
            set
            {
                m_washRoom = value;
                NotifyPropertyChanged();
            }
        }
        public double? Coding
        {
            get => m_coding;
            set
            {
                m_coding = value;
                NotifyPropertyChanged();
            }
        }
        public double? LearnDaily
        {
            get => m_learnDaily;
            set
            {
                m_learnDaily = value;
                NotifyPropertyChanged();
            }
        }
        public double? Eng
        {
            get => m_eng;
            set
            {
                m_eng = value;
                NotifyPropertyChanged();
            }
        }
        public double? Efficiency
        {
            get => m_efficiency;
            set
            {
                m_efficiency = value;
                NotifyPropertyChanged();
            }
        }
        public double? Hz
        {
            get => m_hz;
            set
            {
                m_hz = value;
                NotifyPropertyChanged();
            }
        }
        public double? Score
        {
            get => m_score;
            set
            {
                m_score = value;
                NotifyPropertyChanged();
            }
        }
    }
}
