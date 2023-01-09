using Microsoft.IdentityModel.Tokens;
using OrderManager.Models.Enums;
using OrderManager.Models;
using OrderManager.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderManager.Model;

namespace OrderManager.ViewModels
{
    class DataManageViewModel : INotifyPropertyChanged
    {
        //все заявки
        private List<Order> orders = DataWorker.GetNotDeletedOrders(SearchText);
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
        private List<Order> deletedOrders = DataWorker.GetDeletedOrders(SearchText);
        public List<Order> DeletedOrders
        {
            get { return deletedOrders; }
            set
            {
                deletedOrders = value;
                NotifyPropertyChanged("DeletedOrders");
            }
        }

        public static string ClientName { get; set; }
        public static string PickupPoint { get; set; }
        public static string FinishPoint { get; set; }
        public static string ContactPhone { get; set; }
        public static string SearchText { get; set; }
        public static string ReasonText { get; set; }

        //свойства для выделенных элементов
        public static TabItem SelectedTabItem { get; set; }
        public static Order SelectedOrder { get; set; }
        public static List<StatusHistory> StatusesBySelectedOrder { get; set; }
        public static StatusHistory SelectedStatus { get; set; }

        #region COMMANDS TO ADD
        private RelayCommand addNewOrder;
        public RelayCommand AddNewOrder
        {
            get
            {
                return addNewOrder ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;

                    bool validation = true;
                    string resultString = string.Empty;

                    if (ClientName.IsNullOrEmpty())
                    {
                        validation = false;
                        SetRedBlockControll(window, "NameBlock");
                    }
                    else
                    {
                        SetGrayBlockControll(window, "NameBlock");
                    }

                    if (PickupPoint.IsNullOrEmpty())
                    {
                        validation = false;
                        SetRedBlockControll(window, "PickupPointBlock");
                    }
                    else
                    {
                        SetGrayBlockControll(window, "PickupPointBlock");
                    }

                    if (FinishPoint.IsNullOrEmpty())
                    {
                        validation = false;
                        SetRedBlockControll(window, "FinishPointBlock");
                    }
                    else
                    {
                        SetGrayBlockControll(window, "FinishPointBlock");
                    }

                    if (ContactPhone.IsNullOrEmpty())
                    {
                        validation = false;
                        SetRedBlockControll(window, "ContactPhoneBlock");
                    }
                    else
                    {
                        SetGrayBlockControll(window, "ContactPhoneBlock");
                    }

                    if (validation == true)
                    {
                        resultString = DataWorker.AddOrder(ClientName, PickupPoint, FinishPoint, ContactPhone).Message;
                        ShowMessageToUser(resultString);
                        UpdateOrdersViews();
                        SetNullValuesToProperties();
                        window.Close();
                    }
                });
            }
        }
        #endregion

        #region COMMANDS TO DELETE
        private RelayCommand deleteOrder;
        public RelayCommand DeleteOrder
        {
            get
            {
                return deleteOrder ?? new RelayCommand(obj =>
                {
                    string result = "Ничего не выбрано";

                    if (SelectedTabItem.Name == "OrdersTab" && SelectedOrder != null)
                    {
                        result = DataWorker.DeleteOrder(SelectedOrder.Id).Message;
                    }

                    SetNullValuesToProperties();
                    UpdateOrdersViews();
                    ShowMessageToUser(result);
                });
            }
        }
        private RelayCommand deleteStatus;
        public RelayCommand DeleteStatus
        {
            get
            {
                return deleteStatus ?? new RelayCommand(obj =>
                {
                    string result = "Ничего не выбрано";

                    if (SelectedStatus != null)
                    {
                        result = DataWorker.DeleteStatus(SelectedStatus.Id).Message;
                    }

                    SetNullValuesToProperties();
                    UpdateOrdersViews();
                    UpdateStatusesViews();
                    ShowMessageToUser(result);
                });
            }
        }
        #endregion

        #region COMMANDS TO EDIT
        private RelayCommand editOrder;
        public RelayCommand EditOrder
        {
            get
            {
                return editOrder ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string result = "Заявка не выбрана";
                    if (SelectedOrder != null)
                    {
                        result = DataWorker.EditOrder(SelectedOrder.Id, ClientName, PickupPoint, FinishPoint, ContactPhone).Message;

                        UpdateOrdersViews();
                        SetNullValuesToProperties();
                        ShowMessageToUser(result);
                        window.Close();
                    }
                    else
                    {
                        ShowMessageToUser(result);
                    }
                });
            }
        }
        #endregion

        #region COMMANDS ADD STATUS
        private RelayCommand setInProgressStatus;
        public RelayCommand SetInProgressStatus
        {
            get
            {
                return setInProgressStatus ?? new RelayCommand(obj =>
                {
                    DataWorkerResponse result = DataWorker.AddStatus(OrderStatusEnum.InProgress, StatusesBySelectedOrder.First().OrderId,string.Empty);
                    if (result.IsSuccess == true)
                    {
                        ShowMessageToUser("Статус успешно добавлен!");
                        UpdateOrdersViews();
                        SetNullValuesToProperties();
                        UpdateStatusesViews();
                    }
                    else
                    {
                        ShowMessageToUser(result.Message);
                    }
                });
            }
        }
        private RelayCommand setCompletedStatus;
        public RelayCommand SetCompletedStatus
        {
            get
            {
                return setCompletedStatus ?? new RelayCommand(obj =>
                {
                    DataWorkerResponse result = DataWorker.AddStatus(OrderStatusEnum.Completed, StatusesBySelectedOrder.First().OrderId, string.Empty);
                    if (result.IsSuccess == true)
                    {
                        ShowMessageToUser("Статус успешно добавлен!");
                        UpdateOrdersViews();
                        SetNullValuesToProperties();
                        UpdateStatusesViews();
                    }
                    else
                    {
                        ShowMessageToUser(result.Message);
                    }
                });
            }
        }
        private RelayCommand setCanceledStatus;
        public RelayCommand SetCanceledStatus
        {
            get
            {
                return setCanceledStatus ?? new RelayCommand(obj =>
                {
                    if (ReasonText.IsNullOrEmpty() == true)
                    {
                        ShowMessageToUser("Укажите причину отмены!");
                    }
                    else
                    {
                        DataWorkerResponse result = DataWorker.AddStatus(OrderStatusEnum.Canceled, StatusesBySelectedOrder.First().OrderId, ReasonText);
                        if (result.IsSuccess == true)
                        {
                            ShowMessageToUser("Статус успешно добавлен!");
                            UpdateOrdersViews();
                            SetNullValuesToProperties();
                            UpdateStatusesViews();
                        }
                        else
                        {
                            ShowMessageToUser(result.Message);
                        }
                    }
                });
            }
        }
        #endregion

        #region COMMAND TO SEARCH
        private RelayCommand searchByText;
        public RelayCommand SearchByText
        {
            get
            {
                return searchByText ?? new RelayCommand(obj =>
                {
                    UpdateOrdersViews();
                });
            }
        }

        private RelayCommand clearSearchText;
        public RelayCommand ClearSearchText
        {
            get
            {
                return clearSearchText ?? new RelayCommand(obj =>
                {
                    SearchText = string.Empty;
                    UpdateOrdersViews();
                });
            }
        }

        #endregion

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

        private RelayCommand openEditOrderWnd;
        public RelayCommand OpenEditOrderWnd
        {
            get
            {
                return openEditOrderWnd ?? new RelayCommand(obj =>
                {
                    if (SelectedTabItem.Name == "OrdersTab" && SelectedOrder != null)
                    {
                        if (DataWorker.GetActualStatusByOrderId(SelectedOrder.Id).Status != Models.Enums.OrderStatusEnum.New)
                        {
                            ShowMessageToUser("Запрещено редактировать заявки не в статусе 'Новая'");
                        }
                        else
                        {
                            OpenEditOrderWndMethod(SelectedOrder);
                        }
                    }
                });
            }
        }

        private RelayCommand openEditStatusWnd;
        public RelayCommand OpenEditStatusWnd
        {
            get
            {
                return openEditStatusWnd ?? new RelayCommand(obj =>
                {
                    if (SelectedTabItem.Name == "OrdersTab" && SelectedOrder != null)
                    {
                        List<StatusHistory> statuses = DataWorker.GetStatusesByOrderId(SelectedOrder.Id);
                        OpenEditStatusWndMethod(statuses);
                    }
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
        private void OpenEditOrderWndMethod(Order order)
        {
            EditOrderWindow editOrderWindow = new EditOrderWindow(order);
            SetCenterPositionAndOpen(editOrderWindow);
        }

        //Открытие окна просмотра и редактирования статусов заявки
        private void OpenEditStatusWndMethod(List<StatusHistory> statuses)
        {
            EditStatusWindow editStatusWindow = new EditStatusWindow(statuses);
            SetCenterPositionAndOpen(editStatusWindow);
        }
        private void ShowMessageToUser(string message)
        {
            MessageView messageView = new MessageView(message);
            SetCenterPositionAndOpen(messageView);
        }

        /// <summary>
        /// Размещение окна по центру Главного окна и открытие
        /// </summary>
        /// <param name="window"></param>
        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        #endregion

        #region CHANGE TEXTBLOCK COLOR 
        private void SetRedBlockControll(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }

        private void SetGrayBlockControll(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.LightSlateGray;
        }
        #endregion

        #region UPDATE VIEWS
        private void UpdateOrdersViews()
        {
            Orders = DataWorker.GetNotDeletedOrders(SearchText);
            MainWindow.Orders.ItemsSource = null;
            MainWindow.Orders.Items.Clear();
            MainWindow.Orders.ItemsSource = Orders;
            MainWindow.Orders.Items.Refresh();

            DeletedOrders = DataWorker.GetDeletedOrders(SearchText);
            MainWindow.DeletedOrders.ItemsSource = null;
            MainWindow.DeletedOrders.Items.Clear();
            MainWindow.DeletedOrders.ItemsSource = DeletedOrders;
            MainWindow.DeletedOrders.Items.Refresh();
        }
        private void UpdateStatusesViews()
        {
            StatusesBySelectedOrder = DataWorker.GetStatusesByOrderId(StatusesBySelectedOrder.First().OrderId);
            EditStatusWindow.Statuses.ItemsSource = null;
            EditStatusWindow.Statuses.Items.Clear();
            EditStatusWindow.Statuses.ItemsSource = StatusesBySelectedOrder;
            EditStatusWindow.Statuses.Items.Refresh();
        }
        private void SetNullValuesToProperties()
        {
            ClientName = null;
            PickupPoint = null;
            FinishPoint = null;
            ContactPhone = null;
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
