namespace Vanderkorn.ER.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Электронная приёмная
    /// </summary>
    public class Reception
    {
        /// <summary>
        /// Идентификатор приёмной
        /// </summary>
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ИД министра
        /// </summary>
        public string MinisterId { get; set; }

        /// <summary>
        /// Дата модификации
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Министр
        /// </summary>
        public ApplicationUser Minister { get; set; }

        /// <summary>
        /// Секретари
        /// </summary>

        public virtual ICollection<ApplicationUser> Secretaries { get; set; }
    }
}