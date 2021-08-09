using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>
    /// Сущность справочника ролей
    /// </summary>
    [Table("roles")]
    public class Roles
    {
        /// <summary>
        /// ид роли
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// название роли
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// описание полномочий роли
        /// </summary>
        public string Description { get; set; }
    }
}
