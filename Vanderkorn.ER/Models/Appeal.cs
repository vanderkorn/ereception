namespace Vanderkorn.ER.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Обращение
    /// </summary>
    public class Appeal
    {
        /// <summary>
        /// Идентификатор обращения
        /// </summary>
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор приёмной
        /// </summary>
        public int ReceptionId { get; set; }

        /// <summary>
        /// Приёмная
        /// </summary>
        public virtual Reception Reception { get; set; }

        /// <summary>
        /// Текст обращения
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип обращения
        /// </summary>
        public AppealType AppealType { get; set; }

        /// <summary>
        /// Дата модификации
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Флаг исполнения обращения
        /// </summary>
        public bool IsExecuted { get; set; }

        /// <summary>
        /// Решение министра
        /// </summary>
        public DecisionType DecisionType { get; set; }

        /// <summary>
        /// Текст комментария министра
        /// </summary>
        public string Comment { get; set; }
    }
}