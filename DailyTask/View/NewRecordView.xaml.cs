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
    /// Interaction logic for NewRecordView.xaml
    /// </summary>
    public partial class NewRecordView : Window
    {
        public NewRecordView()
        {
            InitializeComponent();
        }

        public NewRecordView(Daily daily)
        {
            InitializeComponent();
            this.AddRecordWindowDC.DailyRecord = daily;
            this.AddRecordWindowDC.WindowTitleRecordID = "Record: " + daily.Id.ToString();
        }
    }
}
