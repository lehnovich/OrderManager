using OrderManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace OrderManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ListView Orders;
        public static ListView DeletedOrders;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataManageViewModel();
            Orders = ViewOrders;
            DeletedOrders = ViewDeletedOrders;
        }
    }
}
