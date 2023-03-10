using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManager.Models
{
    /// <summary>
    /// Заяка на доставку груза
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Точка вывоза
        /// </summary>
        public string PickupPoint { get; set; }

        /// <summary>
        /// Точка доставки
        /// </summary>
        public string FinishPoint { get; set; }

        /// <summary>
        /// Телефон для связи
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// История статусов
        /// </summary>
        public List<StatusHistory> StatusHitory { get; set; }

        /// <summary>
        /// Удалена ли заявка из основного списка и когда
        /// </summary>
        public DateTime? DeletedDateTime { get; set; }

        /// <summary>
        /// Дата проставления статуса 'Новая'
        /// </summary>
        [NotMapped]
        public DateTime DateStatusNew 
        { 
            get
            {
                return DataWorker.GetStatusNewByOrderId(Id).DataTime;
            }
        }

        /// <summary>
        /// Описание актуального статуса
        /// </summary>
        [NotMapped]
        public string ActualStatus
        {
            get
            {
                return EnumHelper.GetDisplayName(DataWorker.GetActualStatusByOrderId(Id).Status);
            }
        }
    }
}
