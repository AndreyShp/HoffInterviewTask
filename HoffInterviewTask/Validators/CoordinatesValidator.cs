using System;
using Hoff.InterviewTask.Web.Models;
using Hoff.InterviewTask.Web.Validators.Contracts;
using Microsoft.Extensions.Configuration;

namespace Hoff.InterviewTask.Web.Validators
{
    /// <summary>
    /// Отвечает за проверку координат
    /// </summary>
    /// <inheritdoc cref="ICoordinatesValidator"/>
    public class CoordinatesValidator : ICoordinatesValidator
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="config">конфиг с настройками</param>
        public CoordinatesValidator(IConfiguration config)
        {
            _config = config;
        }

        public bool IsValid(PointModel centerPoint, double radius, PointModel point)
        {
            var r = Math.Pow(point.X - centerPoint.X, 2) + Math.Pow(point.Y - centerPoint.Y, 2);
            var radiusSquare = Math.Pow(radius, 2);

            return r <= radiusSquare;
        }
    }
}