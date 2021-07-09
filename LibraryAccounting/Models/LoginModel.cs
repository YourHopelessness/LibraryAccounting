using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Поле Email не может быть путсым")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле пароля не может быть пустым")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
