using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hoff.InterviewTask.CentralBankClient.Contracts;
using Hoff.InterviewTask.CentralBankClient.Contracts.Types;
using Hoff.InterviewTask.CentralBankClient.Exceptions;
using Hoff.InterviewTask.Web.Converters;
using Hoff.InterviewTask.Web.Models;
using Hoff.InterviewTask.Web.Validators.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hoff.InterviewTask.Web.Controllers
{
    public class RatesController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ICoordinatesValidator _coordinatesValidator;
        private readonly IRussiaCentralBankClient _rcbClient;
        private readonly ILogger<RatesController> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="config">конфиг</param>
        /// <param name="coordinatesValidator">отвечает за валидацию координат и попадаение их в окружность</param>
        /// <param name="rcbClient">клиент для работы с ЦБ РФ</param>
        /// <param name="logger">логгер</param>
        public RatesController(IConfiguration config, ICoordinatesValidator coordinatesValidator, IRussiaCentralBankClient rcbClient, ILogger<RatesController> logger)
        {
            _config = config;
            _coordinatesValidator = coordinatesValidator;
            _rcbClient = rcbClient;
            _logger = logger;
        }

        [HttpGet]

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Rates/Coordinate/{x}/{y}")]
        public async Task<IActionResult> GetByCoordinate(double x, double y)
        {
            try
            {
                var point = new PointModel
                            {
                                X = x,
                                Y = y
                            };

                var centerPoint = _config.GetSection("WebSettings:CenterCoordinate").Get<PointModel>();

                var radius = _config.GetValue<double>("WebSettings:Radius");

                if (!_coordinatesValidator.IsValid(centerPoint, radius, point))
                {
                    _logger.Log(LogLevel.Error, $"Переданы координаты x={x}, y={y} которые не попадают в окружность с радиусом {radius}!");

                    return BadRequest($"Переданы координаты которые не попадают в окружность с радиусом {radius}!");
                }

                var date = CoordinatesConverter.ConvertToDate(centerPoint, point);

                var currency = _config.GetValue<Currency>("WebSettings:CurrencyCode");
                var rate = await _rcbClient.GetRateByDate(date, currency);

                var result = new CurrencyResultModel
                             {
                                 Date = date,
                                 Currency = currency,
                                 Rate = rate
                             };

                _logger.Log(LogLevel.Information, $"Для координат x={x}, y={y} получен курс {result.Rate} для валюты {result.Currency} за дату {date:dd.MM.yyyy}");

                return Ok(result);
            }
            catch (CentralBankException e)
            {
                _logger.Log(LogLevel.Error, e, $"Ошибка при попытке получить курс валюты для координат x={x}, y={y}");

                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, $"Неизвестная ошибка при попытке получить курс валюты для координат x={x}, y={y}");

                return BadRequest("Невозможно определить курс. Попробуйте позже или обратитесь в службу поддержки");
            }
        }
    }
}
