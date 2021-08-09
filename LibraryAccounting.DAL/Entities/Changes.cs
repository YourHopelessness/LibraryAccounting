using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>
    /// Сущность изменений
    /// </summary>
    [Table("changes")]
    public class Changes
    {
        /// <summary>
        /// Id сотрудника внесшего изменение
        /// </summary>
        [Required]
        public Guid ChangemakerId { get; set; }
        /// <summary>
        /// Ид книги по которой вносились измененеия
        /// </summary>
        [Required]
        public Guid BookId { get; set; }
        /// <summary>
        /// дата ихмененеия
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ChangeDate { get; set; }
        /// <summary>
        /// комментарий
        /// </summary>
        public string Comment { get; set;  }
    }
}
