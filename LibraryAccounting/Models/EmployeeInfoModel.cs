using System;
using System.Collections.Generic;
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
        public string FullName { get; set; }

        /// <summary>
        /// Рабочая почта
        /// </summary>
        public string WorkEmail { get; set; }

        /// <summary>
        /// Персональная почта
        /// </summary>
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Личный телефон
        /// </summary>
        public string PersonalPhone { get; set; }

    }
}
