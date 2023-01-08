using ManageStaffDBApp.Model;
using OrderManager.Models;
using OrderManager.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderManager.ViewModels
{
    class DataManageViewModel : INotifyPropertyChanged
    {
        //все заявки
        private List<Order> orders = DataWorker.GetNotDeletedOrders();
        public List<Order> Orders 
        { 
            get { return orders; } 
            set 
            { 
                orders = value;
                NotifyPropertyChanged("Orders");
            } 
        }

        //удалённые заявки
        private List<Order> deletedOrders = DataWorker.GetDeletedOrders();
        public List<Order> DeletedOrders
        {
            get { return deletedOrders; }
            set { 
                deletedOrders = value;
                NotifyPropertyChanged("DeletedOrders");
            }
        }

        #region COMMANDS TO OPEN WINDOW
        private RelayCommand openAddNewOrderWnd;
        public RelayCommand OpenAddNewOrderWnd
        {
            get 
            {
                return openAddNewOrderWnd ?? new RelayCommand(obj => 
                { 
                    OpenAddNewOrderWndMethod(); 
                });
            }
        }
        #endregion

        #region METHOS TO OPEN WINDOW
        //Открытие окна новой заяки
        private void OpenAddNewOrderWndMethod()
        { 
            AddNewOrderWindow addNewOrderWindow = new AddNewOrderWindow();
            SetCenterPositionAndOpen(addNewOrderWindow);
        }

        //Открытие окна редактирования заяки
        private void OpenEditOrderWndMethod()
        {
            EditOrderWindow editOrderWindow = new EditOrderWindow();
            SetCenterPositionAndOpen(editOrderWindow);
        }

        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
