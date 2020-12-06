using System;

namespace Hoff.InterviewTask.CentralBankClient.Exceptions
{
    /// <summary>
    /// Исключение для клиентов центральных банков, чтобы скрыть низкоуровневую реализацию
    /// </summary>
    public class CentralBankException : Exception
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">сообщение</param>
        public CentralBankException(string message) : base(message) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">исключение</param>
        public CentralBankException(string message, Exception e) : base(message, e) { }
    }
}
