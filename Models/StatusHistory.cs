using System;
using System.ComponentModel.DataAnnotations.Schema;
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
        /// Уникальный идентификатор заявки 
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Дата и время проставления статуса
        /// </summary>
        public DateTime DataTime { get; set; }

        /// <summary>
        /// Причина отмены (Для статуса Отменена)
        /// </summary>
        public string? CancelReason { get; set; }

        /// <summary>
        /// Дата и время удаления статуса
        /// </summary>
        public DateTime? DeletedDateTime { get; set; }

        /// <summary>
        /// Описание статуса на русском (не мапится бд)
        /// </summary>
        [NotMapped]
        public string RussianName
        {
            get
            {
                return EnumHelper.GetDisplayName(Status);
            }
        }
    }
}
