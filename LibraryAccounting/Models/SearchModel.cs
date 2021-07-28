using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Модель для поиска
    /// </summary>
    public class SearchModel
    {
        /// <summary>
        /// ВВеденная строка поиска
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        ///  Список которому проходит поиск
        /// </summary>
        public SelectList ISBNList { get; set; }

        /// <summary>
        /// Статус книги
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Список статусов для отображения и поиска
        /// </summary>
        public SelectList Statuses { get; set; }
    }
}
