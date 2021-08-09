using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Сущность сотрудника
    /// </summary>
    public class EmployeeInfoModel
    {
        /// <summary>
        /// ID сотрудника
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Полное имя
        /// </summary>
        [DisplayName("ФИО")]
        public string FullName { get; set; }

        /// <summary>
        /// Рабочая почта
        /// </summary>
        [DisplayName("Рабочая почта")]
        public string WorkEmail { get; set; }

        /// <summary>
        /// Персональная почта
        /// </summary>
        [DisplayName("Личная почта")]
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Личный телефон
        /// </summary>
        [DisplayName("Телефон")]
        public string PersonalPhone { get; set; }

        /// <summary>
        /// Должность пользователя
        /// </summary>
        [DisplayName("Должность")]
        public string Position { get; set; }

        /// <summary>
        /// Текущее количетсво книг в использовании
        /// </summary>
        [DisplayName("Взято книг на данный момент")]
        public int CurrentReservationCount { get; set; }

        /// <summary>
        /// Количество книг за все время
        /// </summary>
        [DisplayName("Взято книг за все время")]
        public int AllTimeReservationCount { get; set; }

        /// <summary>
        /// Просроченные книги
        /// </summary>
        [DisplayName("Просрочено книг")]
        public int DelayedBooksCount { get; set; }

    }
}
