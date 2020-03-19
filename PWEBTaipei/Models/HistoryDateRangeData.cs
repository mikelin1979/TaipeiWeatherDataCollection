using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWEBTaipei.Models
{
    public class HistoryDateRangeData
    {
        public string Location { get; set; }
        public List<HistoryTimeRange> TimeRange { get; set; }
    }

    public class HistoryTimeRange
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
