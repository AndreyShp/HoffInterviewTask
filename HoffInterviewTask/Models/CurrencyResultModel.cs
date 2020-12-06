using System;
using Hoff.InterviewTask.CentralBankClient.Contracts.Types;

namespace Hoff.InterviewTask.Web.Models
{
    /// <summary>
    /// Результат определения курса валюты
    /// </summary>
    public class CurrencyResultModel
    {
        /// <summary>
        /// Дату за которую конвертировали курс
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Валюта для которой определяли курс
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Курс валюты к одному рублю РФ
        /// </summary>
        public decimal Rate { get; set; }
    }
}
