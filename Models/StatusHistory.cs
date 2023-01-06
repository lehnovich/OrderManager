using System;
using OrderManager.Models.Enums;

namespace OrderManager.Models
{
    /// <summary>
    /// Статус заявки
    /// </summary>
    public class StatusHistory
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Статус 
        /// </summary>
        public OrderStatusEnum Status { get; set; }

        /// <summary>
        /// Дата и время проставления статуса
        /// </summary>
        public DateTime DataTime { get; set; }
    }
}
