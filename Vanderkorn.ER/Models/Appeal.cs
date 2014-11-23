namespace Vanderkorn.ER.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ���������
    /// </summary>
    public class Appeal
    {
        /// <summary>
        /// ������������� ���������
        /// </summary>
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ������������� �������
        /// </summary>
        public int ReceptionId { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public virtual Reception Reception { get; set; }

        /// <summary>
        /// ����� ���������
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ��� ���������
        /// </summary>
        public AppealType AppealType { get; set; }

        /// <summary>
        /// ���� �����������
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// ���� ���������� ���������
        /// </summary>
        public bool IsExecuted { get; set; }

        /// <summary>
        /// ������� ��������
        /// </summary>
        public DecisionType DecisionType { get; set; }

        /// <summary>
        /// ����� ����������� ��������
        /// </summary>
        public string Comment { get; set; }
    }
}