using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderManager.Models.Enums
{
    /// <summary>
    /// Статусы заявки
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// Новая
        /// </summary>
        [Display(Name = "Новая")]
        New = 10,

        /// <summary>
        /// Передано на выполнение
        /// </summary>
        [Display(Name = "Передана на выполнение")]
        InProgress = 20,

        /// <summary>
        /// Выполнена
        /// </summary>
        [Display(Name = "Выполнена")]
        Completed = 90,

        /// <summary>
        /// Отменена
        /// </summary>
        [Display(Name = "Отменена")]
        Canceled = 95


    }
}
