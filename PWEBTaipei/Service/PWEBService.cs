using Microsoft.Extensions.Configuration;
using PWEBTaipei.DataAccess;
using PWEBTaipei.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PWEBTaipei.Service
{
    public class PWEBService
    {
        /// <summary>
        /// 向PWEB查詢一周預報
        /// </summary>
        /// <returns></returns>
        public QueryPWEBTaipeiWeatherData Query()
        {            
            QueryPWEBTaipeiWeatherData result = new QueryPWEBTaipeiWeatherData();
            using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                var json = client.DownloadString($@"{PWEBSetting.PWEBBaseUrl}{PWEBSetting.PWEBKey}");
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryPWEBTaipeiWeatherData>(json);
            }
            return result;
        }

        /// <summary>
        /// 分析資料並存入DB
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<TaipeiWeather> ParseData(QueryPWEBTaipeiWeatherData data)
        {
            List<TaipeiWeather> result = new List<TaipeiWeather>();

            Locations location = data.records.locations.First();
            var CityName = location.locationsName;
                        
            foreach(var area in location.location)
            {
                var AreaName = area.locationName;

                foreach (var EleType in area.weatherElement)
                {
                    //按照時間區間建立空資料
                    foreach(var TimeRange in EleType.time)
                    {                       
                        var ResultData = result.Where(x => x.EndTime == TimeRange.endTime && x.StartTime == TimeRange.startTime && x.Location == CityName + AreaName).FirstOrDefault();
                        if(ResultData == null)
                        {
                            ResultData = new TaipeiWeather()
                            {
                                Location = CityName + AreaName,
                                EndTime = TimeRange.endTime,
                                StartTime = TimeRange.startTime
                            };

                            result.Add(ResultData);
                        }

                        Byte numval = 0;
                        Byte.TryParse(TimeRange.elementValue.First().value, out numval);
                        switch (EleType.elementName)
                        {
                            case "PoP12h":  //降雨機率                                
                                ResultData.RainChance = numval;
                                break;
                            case "T":       //平均溫度
                                ResultData.Temperature = numval;
                                break;
                            case "RH":      //平均相對濕度
                                ResultData.RelativeHumidity = numval;
                                break;
                            case "MinCI":   //最小舒適度指數
                                ResultData.MinCi = numval;
                                break;
                            case "WS":      //最大風速
                                ResultData.WindSpeed = numval;
                                break;
                            case "MaxAT":   //最高體感溫度
                                ResultData.MaxAt = numval;
                                break;
                            case "Wx":      //天氣現象
                                ResultData.Weather = TimeRange.elementValue.First().value;
                                break;
                            case "MaxCI":   //最大舒適度指數
                                ResultData.MaxCi = numval;
                                break;
                            case "MinT":    //最低溫度
                                ResultData.MinTemperature = numval;
                                break;
                            case "UVI":     //紫外線指數
                                ResultData.Uvi = numval;
                                break;
                            case "WeatherDescription":      //天氣預報綜合描述
                                ResultData.WeatherDescription = TimeRange.elementValue.First().value;
                                break;
                            case "MinAT":   //最低體感溫度
                                ResultData.MinAt = numval;
                                break;
                            case "MaxT":    //最高溫度
                                ResultData.MaxTemperature = numval;
                                break;
                            case "WD":      //風向
                                ResultData.WindDirection = TimeRange.elementValue.First().value;
                                break;
                            case "Td":      //平均露點溫度
                                ResultData.Td = numval;
                                break;
                        }
                    }                    
                }
            }

            using (var db = new PwebtaipeiContext())
            {
                foreach(TaipeiWeather weather in result)
                {
                    if(db.TaipeiWeather.Any(x=>x.Location == weather.Location && x.StartTime == weather.StartTime && x.EndTime == weather.EndTime))
                    {
                        db.TaipeiWeather.Update(weather);
                    } else
                    {
                        db.TaipeiWeather.Add(weather);
                    }
                }
                db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// 從資料庫取得舊預報資料
        /// </summary>
        /// <param name="STTime">起始時間</param>
        /// <param name="EDTime">結束時間</param>
        /// <returns></returns>
        public List<TaipeiWeather> getData(string Location,DateTime STTime,DateTime EDTime)
        {
            List<TaipeiWeather> result = new List<TaipeiWeather>();
            using (var db = new PwebtaipeiContext())
            {
                if (string.IsNullOrEmpty(Location))
                {
                    result = db.TaipeiWeather.Where(x => x.StartTime >= STTime && x.EndTime <= EDTime).ToList();
                }
                else
                {
                    result = db.TaipeiWeather.Where(x => x.Location == Location && x.StartTime >= STTime && x.EndTime <= EDTime).ToList();
                }
                
            }
            return result;  
        }

        /// <summary>
        /// 取得歷史資料中的地區與時間區間
        /// </summary>
        /// <returns></returns>
        public List<HistoryDateRangeData> getHistoryRange()
        {
            List<HistoryDateRangeData> result = new List<HistoryDateRangeData>();
            using (var db = new PwebtaipeiContext())
            {
                var data = db.TaipeiWeather.Select(x=>new { x.Location,x.StartTime,x.EndTime }).ToList();
                result = data.Select(x => new HistoryDateRangeData() { Location = x.Location }).ToList();
                foreach(var location in result)
                {
                    location.TimeRange = data.Where(x => x.Location == location.Location).Select(x => new HistoryTimeRange() { StartTime = x.StartTime, EndTime = x.EndTime }).ToList();
                }
            }
            return result;
        }

        public bool checkNew()
        {
            bool result;
            using (var db = new PwebtaipeiContext())
            {
                result = (db.TaipeiWeather.Any(x => x.StartTime > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
            }

            return result;
        }
    }
}
