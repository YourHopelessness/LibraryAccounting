using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.BL.Dto
{
    /// <summary> Личная карточка читателя</summary>
    public class ReadersDto
    {
        /// идентификатор читателя
        public Guid ReaderId { get; set; }

        /// имя читателя
        public string ReaderName { get; set; }

        /// рабочая почта
        public string ReaderWorkEmail { get; set; }

        /// личная почта
        public string ReaderPersonalEmail { get; set; }

        /// телефон
        public string ReaderPhone { get; set; }

        /// должность читателя
        public string ReaderPosition { get; set; }
    }
}
