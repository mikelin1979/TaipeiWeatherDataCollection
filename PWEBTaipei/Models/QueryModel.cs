using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWEBTaipei.Models
{
    public class QueryModel
    {
        public string location { get; set; } = "";
        public DateTime? sttime { get; set; }
        public DateTime? edtime { get; set; }
    }
}
