namespace Hoff.InterviewTask.CentralBankClient
{
    /// <summary>
    /// Настройки для ЦБ
    /// </summary>
    public class CentralBankSettings
    {
        /// <summary>
        /// Адрес подключения
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Кол-во попыток
        /// </summary>
        public int CountTries { get; set; }
    }
}
