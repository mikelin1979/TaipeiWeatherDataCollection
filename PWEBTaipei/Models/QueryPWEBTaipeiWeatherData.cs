using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PWEBTaipei.Models
{
    public class QueryPWEBTaipeiWeatherData
    {
        public RecordItem records { get; set; }
    }

    public class RecordItem
    {
        public List<Locations> locations { get; set; }

    }

    public class Locations
    {
        public string datasetDescription { get; set; }
        public string locationsName { get; set; }
        public string dataid { get; set; }
        public List<Location> location { get; set; }
    }

    public class Location
    {
        public string locationName { get; set; }
        public List<WeatherElement> weatherElement { get; set; }
    }

    public class WeatherElement
    {
        public string elementName { get; set; }
        public string description { get; set; }
        public List<TimeRange> time { get; set; }
    }

    public class TimeRange
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public List<DataElement> elementValue { get; set; }
    }

    public class DataElement
    {
        public string value { get; set; }
        public string measures { get; set; }
    }
}
