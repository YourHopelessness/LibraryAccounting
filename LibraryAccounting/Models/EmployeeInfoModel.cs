using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    public class EmployeeInfoModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string PersonalPhone { get; set; }

    }
}
