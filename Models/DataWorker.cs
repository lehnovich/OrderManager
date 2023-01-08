using Microsoft.IdentityModel.Tokens;
using OrderManager.Models.Data;
using OrderManager.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Models
{
    public static class DataWorker
    {
        /// <summary>
        /// Получить список неудалённых заявок
        /// </summary>
        public static List<Order> GetNotDeletedOrders()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Order> result = db.Orders.Where(o => o.DeletedDateTime == null).ToList();
                return result;
            }
        }

        /// <summary>
        /// Получить список удалённых заявок
        /// </summary>
        /// <returns></returns>
        public static List<Order> GetDeletedOrders()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Order> result = db.Orders.Where(o => o.DeletedDateTime != null).ToList();
                return result;
            }
        }

        /// <summary>
        /// Добавление новой заявки
        /// </summary>
        /// <param name="clientName">Имя клиента</param>
        /// <param name="pickPoint">Адрес точки пикапа</param>
        /// <param name="finishPoint">Адрес точки доставки</param>
        /// <param name="pickupDay">Дата пикапа</param>
        /// <param name="contactPhone">Контактный телефон</param>
        /// <returns></returns>
        public static DataWorkerResponse AddOrder(string clientName, string pickPoint, string finishPoint, string contactPhone)
        {
            DataWorkerResponse result = new DataWorkerResponse();

            using (ApplicationContext db = new ApplicationContext())
            {
                Order newOrder = new Order {
                    ClientName = clientName, 
                    PickupPoint = pickPoint, 
                    FinishPoint = finishPoint, 
                    ContactPhone = contactPhone
                };

                StatusHistory newStatus = new StatusHistory { 
                    Status = OrderStatusEnum.New, 
                    DataTime = DateTime.Now 
                };

                db.Orders.Add(newOrder);

                DataWorkerResponse statusResponse = AddStatus(OrderStatusEnum.New, newOrder.Id, null);

                if (statusResponse.IsSuccess == false)
                {
                    result = statusResponse;
                    return result;
                }

                db.SaveChanges();

                return result;
            }
        }
    
        /// <summary>
        /// Редактирование заявки
        /// Редактирование допускается только заявок в статусе New
        /// </summary>
        /// <param name="orderId">Идентификатор заявки</param>
        /// <param name="newClientName">Новое имя клиента</param>
        /// <param name="newPickupPoint">Новая точка пикапа</param>
        /// <param name="newFinishPoint">Новая точка доставки</param>
        /// <param name="newPickupDay">Новый день пикапа</param>
        /// <param name="newContactPhone">Новый телефон для связи</param>
        /// <returns></returns>
        public static DataWorkerResponse EditOrder(int orderId, string newClientName, string newPickupPoint, string newFinishPoint, string newContactPhone) 
        {
            DataWorkerResponse result = new DataWorkerResponse();

            using (ApplicationContext db = new ApplicationContext())
            {
                //Ищем запись для редактирования
                Order? order = db.Orders.Where(o => o.Id == orderId && o.DeletedDateTime == null).FirstOrDefault();

                if (order == null)
                {
                    result.IsSuccess = false;
                    result.Message = $"Ошибка при редактировании заявки с номером = {orderId}.";
                    return result;
                }

                //Редактирование заявки допускается только, если заявка находится в статусе «Новая» 
                bool isNotNewExist = db.StatusHistories.Where(s => s.OrderId == orderId && s.DeletedDateTime == null && s.Status != OrderStatusEnum.New).Any();

                if (isNotNewExist == true) 
                {
                    result.IsSuccess = false;
                    result.Message = $"Редактирование заказов не в статусе New запрещено";
                    return result;
                }

                order.ClientName = newClientName;
                order.PickupPoint = newPickupPoint;
                order.FinishPoint = newFinishPoint;
                order.ContactPhone = newContactPhone;
                db.SaveChanges();
            }

            return result;
        }

        /// <summary>
        /// Удаление заявки.
        /// Удаление происходит фиктивно, путём изменения поля DeletedDateTime.  
        /// </summary>
        /// <param name="orderId">Идентификатор заявки</param>
        /// <returns></returns>
        public static DataWorkerResponse DeleteOrder(int orderId)
        {
            DataWorkerResponse result = new DataWorkerResponse();

            using (ApplicationContext db = new ApplicationContext())
            {
                //Ищем запись для удаления
                Order? order = db.Orders.Where(o => o.Id == orderId && o.DeletedDateTime == null).FirstOrDefault();

                if (order == null)
                {
                    result.IsSuccess = false;
                    result.Message = $"Ошибка при удалении заявки с номером = {orderId}.";
                    return result;
                }

                order.DeletedDateTime = DateTime.Now;
                db.SaveChanges();
            }

            return result;
        }

        /// <summary>
        /// Получить список неудалённых статусов по индентификатору заявки
        /// </summary>
        /// <returns></returns>
        public static List<StatusHistory> GetStatusesByOrderId(int orderId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<StatusHistory> result = db.StatusHistories.Where(s => s.DeletedDateTime != null && s.OrderId == orderId).ToList();
                return result;
            }
        }

        /// <summary>
        /// Добавление нового статуса к заявке
        /// </summary>
        /// <param name="status">Новый статус</param>
        /// <param name="orderId">Идентификатор заявки</param>
        /// <param name="reason">Причина отмены (для статуса Отменена)</param>
        public static DataWorkerResponse AddStatus(OrderStatusEnum status, int orderId, string? reason)
        {
            DataWorkerResponse result = new DataWorkerResponse();

            using (ApplicationContext db = new ApplicationContext())
            {
                List<StatusHistory> statusHistories = db.StatusHistories.Where(s => s.OrderId == orderId && s.DeletedDateTime == null).ToList();

                DataWorkerResponse valiateResult = ValidateStatus(statusHistories, status);

                if (valiateResult.IsSuccess == false)
                {
                    result = valiateResult;
                    return result;
                }
                
                if (status == OrderStatusEnum.Canceled && reason.IsNullOrEmpty())
                {
                    result.IsSuccess = false;
                    result.Message = $"Для установки статуса Отменена, требуется обязательно указать причину отмены";
                    return result;
                }

                StatusHistory newStatus = new StatusHistory { Status = status, OrderId = orderId, DataTime = DateTime.Now, CancelReason = reason };
                db.StatusHistories.Add(newStatus);
                db.SaveChanges();

                return result;
            }
        }

        /// <summary>
        /// Удаление статуса.
        /// Удаление происходит фиктивно, путём изменения поля DeletedDateTime.  
        /// </summary>
        /// <param name="statusId">Идентификатор статуса</param>
        /// <returns></returns>
        public static DataWorkerResponse DeleteStatus(int statusId)
        {
            DataWorkerResponse result = new DataWorkerResponse();

            using (ApplicationContext db = new ApplicationContext())
            {
                //Ищем запись для удаления
                StatusHistory? status = db.StatusHistories.Where(s => s.Id == statusId && s.DeletedDateTime == null).FirstOrDefault();

                if (status == null)
                {
                    result.IsSuccess = false;
                    result.Message = $"Ошибка при удалении статуса с идентификатором = {statusId}.";
                    return result;
                }

                status.DeletedDateTime = DateTime.Now;
                db.SaveChanges();
            }

            return result;
        }

        /// <summary>
        /// Валидация логики проставления статусов
        /// </summary>
        /// <param name="orderStatuses">Список существующих статусов</param>
        /// <param name="newStatus">Новый статус</param>
        /// <returns></returns>
        private static DataWorkerResponse ValidateStatus(List<StatusHistory> orderStatuses, OrderStatusEnum newStatus)
        {
            DataWorkerResponse result = new DataWorkerResponse();

            //Статусы не могут быть проставлены при наличии статуса отмены.
            bool checkIsCancelStatusExist = orderStatuses.Any(s => s.Status == OrderStatusEnum.Canceled);
            if (checkIsCancelStatusExist == true)
            {
                result.IsSuccess = false;
                result.Message = "Невозможно изменить статус у удалённой заявки.";
                return result;
            }

            if (newStatus == OrderStatusEnum.New)
            {
                //Статус New не может дублироваться и не может быть установлен при наличии других статусов
                bool checkIsAnyStatusExist = orderStatuses.Any();
                if (checkIsAnyStatusExist == true)
                {
                    result.IsSuccess = false;
                    result.Message = "Невозможно добавить статус 'Новый' при наличии других статусов.";
                    return result;
                }
            }

            if (newStatus == OrderStatusEnum.InProgress)
            {
                //Статус InProgress не может быть установлен при наличии статуса завершения
                bool checkIsProgressStatusExist = orderStatuses.Any(s => s.Status == OrderStatusEnum.Completed);
                if (checkIsProgressStatusExist == true)
                {
                    result.IsSuccess = false;
                    result.Message = "Невозможно добавить статус 'Передано на выполнение' при наличии статуса 'Выполнено'.";
                    return result;
                }
            }

            if (newStatus == OrderStatusEnum.Completed)
            {
                //Статус Completed не может дублироваться
                bool checkIsCompletedExist = orderStatuses.Any(s => s.Status == OrderStatusEnum.Completed);
                if (checkIsCompletedExist == true)
                {
                    result.IsSuccess = false;
                    result.Message = "Невозможно добавить статус 'Выполнена' более одного раза.";
                    return result;
                }
            }

            if (newStatus == OrderStatusEnum.Canceled)
            {
                //Статус Canceled дублироваться не может
                bool checkIsCanceledExist = orderStatuses.Any(s => s.Status == OrderStatusEnum.Canceled);
                if (checkIsCanceledExist == true)
                {
                    result.IsSuccess = false;
                    result.Message = "Невозможно добавить статус 'Отменена' более одного раза.";
                    return result;
                }
            }

            return result;
        }
    }
}
