using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWEBTaipei.Models
{
    public class LineChartViewModel
    {
        /// <summary>
        /// 顯示日期與風向、天氣等資訊
        /// </summary>
        public List<List<string>> Label { get; set; }
        /// <summary>
        /// 溫度、舒適值、濕度等數值線
        /// </summary>
        public List<string> series { get; set; }
        /// <summary>
        /// 數值資料
        /// </summary>
        public List<List<byte>> data { get; set; }
        /// <summary>
        /// 歷史資料地區
        /// </summary>
        public List<string> Area { get; set; }
        /// <summary>
        /// 歷史資料查詢起始時間
        /// </summary>
        public List<string> ListStartTime { get; set; }
        /// <summary>
        /// 歷史資料查詢結束時間
        /// </summary>
        public List<string> ListEndTime { get; set; }
    }

    
}
