namespace OrderManager.Models
{
    /// <summary>
    /// Клиент
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование организации клиента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Контактный телефон клиента
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// ИНН клиента
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// КПП клиента
        /// </summary>
        public string? Kpp { get; set; }

        /// <summary>
        /// Поле, указывающее на то, активен ли клиент
        /// </summary>
        public bool IsActive { get; set; }
    }
}
