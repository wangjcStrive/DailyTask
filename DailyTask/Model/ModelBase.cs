using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DailyTask.Model
{
    //todo.
    //新添加的record要同步到住窗口，record base已经继承了ModelBase，不能同时继承EventArgs，所以加到了这里...
    public class ModelBase : EventArgs, INotifyPropertyChanged
    {
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
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
