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
        New = 10,
        
        /// <summary>
        /// Передано на выполнение
        /// </summary>
        InProgress = 20,

        /// <summary>
        /// Выполнена
        /// </summary>
        Completed = 90,

        /// <summary>
        /// Отменена
        /// </summary>
        Canceled = 95
    }
}
