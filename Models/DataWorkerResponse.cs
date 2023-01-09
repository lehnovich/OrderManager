namespace OrderManager.Models
{
    /// <summary>
    /// Модель ответа на работу DataWorker
    /// </summary>
    public class DataWorkerResponse
    {
        /// <summary>
        /// Выполнен ли запрос
        /// </summary>
        public bool IsSuccess { get; set; } = true;
        
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; } = "Успешно выполнено!";
    }
}
