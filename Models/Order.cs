using System;
using System.Collections.Generic;

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
        public Client Client { get; set; }

        /// <summary>
        /// Точка вывоза
        /// </summary>
        public string PickupPoint { get; set; }

        /// <summary>
        /// Точка доставки
        /// </summary>
        public string FinishPoint { get; set; }

        /// <summary>
        /// День отгрузки
        /// </summary>
        public DateTime PickupDay { get; set; }

        /// <summary>
        /// История статусов
        /// </summary>
        public List<StatusHistory> StatusHitory { get; set; }

        /// <summary>
        /// Удалена ли запись из основного списка
        /// </summary>
        public DateTime? DeleteDateTime { get; set; }
    }
}
