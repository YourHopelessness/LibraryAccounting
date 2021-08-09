using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>
    /// Сущность сотрудников
    /// </summary>
    [Table("employees")]
    public class Employees
    {
        /// <summary>
        /// ид сотрудника
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Фвмилия
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        [Required]
        public string Position { get; set; }
        /// <summary>
        /// Рабочая почта
        /// </summary>
        [Required]
        public string WorkEmail { get; set; }
        /// <summary>
        /// личная почта
        /// </summary>
        public string PersonalEmail { get; set; }
        /// <summary>
        /// телефонный номер
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
