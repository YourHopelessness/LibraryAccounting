using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Сущность страницы входа
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required(ErrorMessage = "Имя пользователя не может быть пустым")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Поле пароля не может быть пустым")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
