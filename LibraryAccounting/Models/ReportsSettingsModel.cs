using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Модель для составления отчетов
    /// </summary>
    public class ReportsSettingsModel
    {
        /// <summary>
        /// Тип отчета
        /// </summary>
        [Display(Name = "Выберите тип составляемого отчета")]
        public SelectList ReportType { get; set; }

        /// <summary>
        /// Выбранный тип
        /// </summary>
        public string CurrentReportType { get; set; }

        /// <summary>
        /// Период составления отчета
        /// </summary>
        [Display(Name = "Выберите период")]
        public SelectList Period { get; set; }

        /// <summary>
        /// Выбранный период
        /// </summary>
        public string CurrentPeriod { get; set; }

        /// <summary>
        /// Начало выбранного периода, если выбран конкретный период
        /// </summary>
        [Required(ErrorMessage = "Дата начала периода составления отчета не может быть пустой")]
        [Display(Name = "Выберите дату начала")]
        public DateTime StartReportDate { get; set; }

        /// <summary>
        /// Окночнание выбранного периода, если выбран конкретный период
        /// </summary>
        [Required(ErrorMessage = "Дата окончания периода составления отчета не может быть пустой")]
        [Display(Name = "Выберите дату окончания")]
        public DateTime StopReportDate { get; set; }

        /// <summary>
        /// Параметры, добавлемые в отчет
        /// </summary>
        [Display(Name = "Параметры отчета")]
        public List<List<string>> Propetries { get; set; }

        ///<inheritdoc></inheritdoc>
        public ReportsSettingsModel()
        {
            ReportType = new SelectList(new List<string>{ "Отчет по пользователям", "Отчет по книгам" });
            Period = new SelectList((new List<string> { "Текущее состояние", "За все время", "Выбранный период" }));
            Propetries = new List<List<string>>();
        }

    }
}
