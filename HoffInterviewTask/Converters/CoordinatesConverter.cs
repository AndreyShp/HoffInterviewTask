using System;
using Hoff.InterviewTask.Web.Models;

namespace Hoff.InterviewTask.Web.Converters
{
    /// <summary>
    /// Отвечает за перевод координат в нужные единицы
    /// </summary>
    public class CoordinatesConverter
    {
        /// <summary>
        /// Переводит координату в дату в зависимости в какую четверть окружности папала координата
        /// </summary>
        /// <param name="centerPoint">координата с центром окружности</param>
        /// <param name="point">координата попадание которой нужно проверить</param>
        /// <returns>дата соответствующая координате</returns>
        public static DateTime ConvertToDate(PointModel centerPoint, PointModel point)
        {
            var today = DateTime.Today;
            if (point.X < centerPoint.X && point.Y > centerPoint.Y)
            {
                return today.AddDays(-1);
            }

            if (point.X < centerPoint.X && point.Y < centerPoint.Y)
            {
                return today.AddDays(-2);
            }

            if (point.X > centerPoint.X && point.Y < centerPoint.Y)
            {
                return today.AddDays(1);
            }

            return today;
        }
    }
}