using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace DailyTask.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //protected void NotifyPropertyChanged(string propertyName)
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            if (Context != null)
            {
                Context.Dispatcher.BeginInvoke(new Action(() =>
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }));
            }
            else
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public DependencyObject Context { get; set; }
    }
}
