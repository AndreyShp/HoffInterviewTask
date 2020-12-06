using Hoff.InterviewTask.Web.Models;

namespace Hoff.InterviewTask.Web.Validators.Contracts
{
    /// <summary>
    /// Интерфейс валидатора проверки координат
    /// </summary>
    public interface ICoordinatesValidator
    {
        /// <summary>
        /// Проверяет попадает ли координата в окружность с радиусом
        /// </summary>
        /// <param name="centerPoint">координата с центром окружности</param>
        /// <param name="radius">радиус окружности</param>
        /// <param name="point">координата попадание которой нужно проверить</param>
        /// <returns>true если координата попала в заданную окружность</returns>
        bool IsValid(PointModel centerPoint, double radius, PointModel point);
    }
}
