using PWEBTaipei.Models;
using PWEBTaipei.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWEBTaipei.Bussiness
{
    public class PWEBAgent
    {
        private PWEBService service = new PWEBService(); 
        /// <summary>
        /// 按照指定日期取得資料庫中的舊資料，如果沒有指定日期，則向PWEB取得當日起一周預報
        /// </summary>
        /// <param name="location">區</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public LineChartViewModel get(string location = null, DateTime? STdate = null, DateTime? EDdate = null)
        {
            List<TaipeiWeather> dataresult = new List<TaipeiWeather>();
            if((STdate == null) || (EDdate == null))
            {
                if (!service.checkNew())
                {
                    //未指定日期且無新資料，向PWEB取一周預報資料
                    QueryPWEBTaipeiWeatherData data = service.Query();
                    //解析預報資料
                    dataresult = service.ParseData(data);
                }else
                {
                    //已有資料可從資料庫取得，避免頻繁向PWEB取資料拖慢效能
                    dataresult = service.getData(location, DateTime.Now, DateTime.Now.AddDays(7));
                }
                              

            } else
            {
                //按照指定日期區間從資料庫取出資料
                dataresult = service.getData(location, STdate.Value, EDdate.Value);
            }

            if(string.IsNullOrEmpty(location))
            {
                //由於未指定地區，將從第一筆的地區做為預設
                dataresult = dataresult.Where(x => x.Location == dataresult.FirstOrDefault().Location).ToList();
            } else
            {
                dataresult = dataresult.Where(x => x.Location == location).ToList();
            }

            LineChartViewModel result = new LineChartViewModel();
            //取得歷史資料中的地區以及時間區間供前端選擇
            var HistoryArea = service.getHistoryRange();
            //轉換成Viewmodel
            result.Area = HistoryArea.Select(x => x.Location).Distinct().ToList();
            var range = HistoryArea.SelectMany(x => x.TimeRange).ToList();
            result.ListStartTime = range.Distinct().Select(x => x.StartTime.ToString("yyyy-MM-dd HH:mm:ss")).Distinct().ToList();
            result.ListEndTime = range.Distinct().Select(x => x.EndTime.ToString("yyyy-MM-dd HH:mm:ss")).Distinct().ToList();

            result.series = new List<string>() { "降雨機率", "平均溫度", "平均相對濕度", "最小舒適度指數",
                "最大風速", "最高體感溫度", "最大舒適度指數","最低溫度","紫外線指數",
                "最低體感溫度","最高溫度","平均露點溫度" };

            List<List<string>> Labels = new List<List<string>>();            
            foreach (var dt in dataresult)
            {
                List<string> datelabel = new List<string>();
                datelabel.Add(dt.StartTime.ToString("yyyy-MM-dd hh:mm:ss ~"));
                datelabel.Add(dt.EndTime.ToString("yyyy-MM-dd hh:mm:ss"));
                datelabel.Add(dt.Weather);
                //datelabel.Add(dt.WeatherDescription); //加這行會跑版
                datelabel.Add(dt.WindDirection);

                Labels.Add(datelabel);

            }
            result.Label = Labels;

            List<List<byte>> DataVals = new List<List<byte>>();
            DataVals.Add(dataresult.Select(x => x.RainChance).ToList()); //降雨機率
            DataVals.Add(dataresult.Select(x => x.Temperature).ToList()); //平均溫度
            DataVals.Add(dataresult.Select(x => x.RelativeHumidity).ToList()); //平均相對濕度
            DataVals.Add(dataresult.Select(x => x.MinCi).ToList()); //最小舒適度指數
            DataVals.Add(dataresult.Select(x => x.WindSpeed).ToList()); //最大風速
            DataVals.Add(dataresult.Select(x => x.MaxAt).ToList()); //最高體感溫度
            DataVals.Add(dataresult.Select(x => x.MaxCi).ToList()); //最大舒適度指數
            DataVals.Add(dataresult.Select(x => x.MinTemperature).ToList()); //最低溫度
            DataVals.Add(dataresult.Select(x => x.Uvi).ToList()); //紫外線指數
            DataVals.Add(dataresult.Select(x => x.MinAt).ToList()); //最低體感溫度
            DataVals.Add(dataresult.Select(x => x.MaxTemperature).ToList()); //最高溫度
            DataVals.Add(dataresult.Select(x => x.Td).ToList()); //平均露點溫度

            result.data = DataVals;            
            
            return result;
        }
    }
}
