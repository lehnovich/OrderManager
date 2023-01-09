using OrderManager.Models;
using OrderManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrderManager.Views
{
    /// <summary>
    /// Interaction logic for EditStatusWindow.xaml
    /// </summary>
    public partial class EditStatusWindow : Window
    {

        public static ListView Statuses;
        public EditStatusWindow(List<StatusHistory> statuses)
        {
            InitializeComponent();
            DataContext = new DataManageViewModel();
            DataManageViewModel.StatusesBySelectedOrder = statuses;
            Statuses = ViewStatuses;
        }
    }
}
