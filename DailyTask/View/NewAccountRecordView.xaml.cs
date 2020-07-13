using DailyTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DailyTask.View
{
    /// <summary>
    /// Interaction logic for NewAccountRecordView.xaml
    /// </summary>
    public partial class NewAccountRecordView : Window
    {
        public NewAccountRecordView()
        {
            InitializeComponent();
        }
        public NewAccountRecordView(AccountRecord newRecord)
        {
            InitializeComponent();
            this.AccountModifyVM.AccountRecordToModify = newRecord;
        }
    }
}
