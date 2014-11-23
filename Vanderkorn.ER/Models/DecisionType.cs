namespace Vanderkorn.ER.Models
{
    /// <summary>
    /// Тип решения
    /// </summary>
    public enum DecisionType
    {
        /// <summary>
        /// Не принято решение
        /// </summary>
        None,
        /// <summary>
        /// Принять посетителя
        /// </summary>
        Accept,
        /// <summary>
        /// Отказать посетителю
        /// </summary>
        Reject,
        /// <summary>
        /// Перезвонить посетителю
        /// </summary>
        CallBack
    }
}