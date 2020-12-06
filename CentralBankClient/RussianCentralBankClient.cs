using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using CbrNative;
using Hoff.InterviewTask.CentralBankClient.Contracts;
using Hoff.InterviewTask.CentralBankClient.Contracts.Types;
using Hoff.InterviewTask.CentralBankClient.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Hoff.InterviewTask.CentralBankClient
{
    /// <summary>
    /// Клиент для работы с API ЦБ России
    /// </summary>
    /// <inheritdoc cref="IRussiaCentralBankClient" />
    public class RussianCentralBankClient : IRussiaCentralBankClient
    {
        private CentralBankSettings _settings;

        /// <summary>
        /// Конструктор
        /// </summary>
        public RussianCentralBankClient() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settings">настройки клиента</param>
        public RussianCentralBankClient(CentralBankSettings settings)
        {
            _settings = settings;
        }

        public async Task<decimal> GetRateByDate(DateTime date, Currency currency)
        {
            return await SafetyAction(async client =>
                         {
                             var rawXml = await client.GetCursOnDateAsync(date);

                             foreach (XElement childNote in rawXml.Nodes)
                             {
                                 if (!childNote.Name.LocalName.Equals("diffgram", StringComparison.InvariantCultureIgnoreCase))
                                 {
                                     continue;
                                 }

                                 var valuteData = childNote.Element("ValuteData");

                                 if (valuteData == null)
                                 {
                                     throw new CentralBankException("Изменился формат ответа у ЦБ РФ - не найдены курсы валют");
                                 }

                                 var rawRates = valuteData.Elements("ValuteCursOnDate");

                                 foreach (var rawRate in rawRates)
                                 {
                                     var rawCode = rawRate.Element("Vcode")?.Value;
                                     if (string.IsNullOrEmpty(rawCode) || !int.TryParse(rawCode, out var code))
                                     {
                                         throw new CentralBankException($"Изменился формат ответа у ЦБ РФ - не смогли распарсить код валюты {rawCode}");
                                     }

                                     if (currency != (Currency) code)
                                     {
                                         continue;
                                     }

                                     var rawCurs = rawRate.Element("Vcurs")?.Value;
                                     if (string.IsNullOrEmpty(rawCurs) || !decimal.TryParse(rawCurs, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                                     {
                                         throw new CentralBankException($"Изменился формат ответа у ЦБ РФ - не можем распарсить курс валюты {rawCurs}");
                                     }

                                     return result;
                                 }
                             }

                             throw new CentralBankException($"Не найден курс валюты для {currency} за {date:dd.MM.yyyy}");
                         });
        }

        private async Task<T> SafetyAction<T>(Func<DailyInfoSoapClient, Task<T>> func)
        {
            if (_settings == null)
            {
                _settings = LoadSettings();
            }

            Exception e;
            var tryNumber = 1;
            do
            {
                try
                {
                    var binding = new DailyInfoSoapClient.EndpointConfiguration();
                    var client = new DailyInfoSoapClient(binding, _settings.EndPoint);

                    return await func(client);
                } 
                catch (CentralBankException)
                {
                    throw;
                } 
                catch (Exception ex)
                {
                    e = ex;
                }
            } while (++tryNumber <= _settings.CountTries);

            throw new CentralBankException("Не смогли получить курс с ЦБ РФ", e);
        }

        private static CentralBankSettings LoadSettings()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("CentralBankClient.json", optional: false, reloadOnChange: true);

            var root = configurationBuilder.Build();
            var settingsSection = root.GetSection("russianCentralBankSettings");

            if (!int.TryParse(settingsSection.GetSection("countTries").Value, out var countTries)) {
                countTries = 3;
            }

            var settings = new CentralBankSettings
                           {
                               EndPoint = settingsSection.GetSection("endPoint").Value,
                               CountTries = countTries
                           };
            return settings;
        }
    }
}