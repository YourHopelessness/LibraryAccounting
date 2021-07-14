using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    public class SearchModel
    {
        public string SearchString { get; set; }
        public SelectList ISBNList { get; set; }
        public string Status { get; set; }
        public SelectList Statuses { get; set; }
    }
}
