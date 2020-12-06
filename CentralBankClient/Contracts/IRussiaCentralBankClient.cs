using System;
using System.Threading.Tasks;
using Hoff.InterviewTask.CentralBankClient.Contracts.Types;

namespace Hoff.InterviewTask.CentralBankClient.Contracts
{
    /// <summary>
    /// Интерфейс клиента для работы с API ЦБ России
    /// </summary>
    public interface IRussiaCentralBankClient
    {
        /// <summary>
        /// Возвращает курс иностранной валюты к одному рублю РФ
        /// </summary>
        /// <param name="date">дата за которую нужно получить курс</param>
        /// <param name="currency">валюта для которой нужно получить курс</param>
        /// <returns></returns>
        Task<decimal> GetRateByDate(DateTime date, Currency currency);
    }
}
