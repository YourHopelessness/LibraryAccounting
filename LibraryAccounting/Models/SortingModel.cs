using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Сортировка
    /// </summary>
    public class SortingModel<T> where T : class
    {
        /// <summary>
        /// Поле сортировки
        /// </summary>
        public List<string> SortedField { get; set; }

        private readonly T _model;

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        [DefaultValue(true)]
        public bool Ascending { get; set; }

        public SortingModel()
        {
            SortedField = typeof(T).GetProperties().Select(p => p.Name).ToList();
        }
    }
}
