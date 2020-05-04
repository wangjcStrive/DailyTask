using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DailyTask.Model
{
    public class ModelBase
    {
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
