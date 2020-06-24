using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherData.ViewComponents;

namespace WeatherData.Models
{
    public class SimpleWeatherService
    {
        //TODO 宜改用 HttpClientFactory https://blog.darkthread.net/blog/httpclient-sigleton/
        static readonly HttpClient httpClient = new HttpClient();
        //TODO 實務應用時，URL 應移至 appSettings.json
        static readonly string openDataApiUrl = "https://opendata.cwb.gov.tw/fileapi/v1/opendataapi/F-C0032-001?Authorization=CWB-44E41E80-FB22-4687-B078-B38C36C8A96A&downloadType=WEB&format=JSON";
        public class WeatherData
        {
            public string ZoneName { get; set; }
            public string Status { get; set; }
            public string MaxTemp { get; set; }
            public string MinTemp { get; set; }
        }
        public WeatherData GetTaipeiWeatherFromOpenDataApi()
        {
            //TODO 應改為 async/await
            var json = httpClient.GetAsync(openDataApiUrl).Result.Content.ReadAsStringAsync().Result;
            //https://blog.darkthread.net/blog/httpclient-sigleton/
            using (var doc = JsonDocument.Parse(json,
                new JsonDocumentOptions { AllowTrailingCommas = true }))
            {
                var taipeiData = doc
                    .RootElement
                    .GetProperty("cwbopendata")
                    .GetProperty("dataset")
                    .GetProperty("location")
                    .EnumerateArray()
                    .Single(o => o.GetProperty("locationName").GetString() == "臺北市")
                    .GetProperty("weatherElement")
                    .EnumerateArray();
                Func<string, string> readParameterName =
                    (elemName) =>
                    taipeiData.Single(o => o.GetProperty("elementName").GetString() == elemName)
                        .GetProperty("time").EnumerateArray().First()
                        .GetProperty("parameter").GetProperty("parameterName").GetString();
                return new WeatherData
                {
                    Status = readParameterName("Wx"),
                    MaxTemp = readParameterName("MaxT"),
                    MinTemp = readParameterName("MinT")
                };
            }
        }

        public WeatherData GetWeatherFromOpenDataApi(string zoneName)
        {
            var json = httpClient.GetAsync(openDataApiUrl).Result.Content.ReadAsStringAsync().Result;
            //https://blog.darkthread.net/blog/httpclient-sigleton/
            using (var doc = JsonDocument.Parse(json,
                new JsonDocumentOptions { AllowTrailingCommas = true }))
            {
                var taipeiData = doc
                    .RootElement
                    .GetProperty("cwbopendata")
                    .GetProperty("dataset")
                    .GetProperty("location")
                    .EnumerateArray()
                    //TODO: 省略比對不到縣市名稱之錯誤處理
                    .Single(o => o.GetProperty("locationName").GetString() == zoneName)
                    .GetProperty("weatherElement")
                    .EnumerateArray();
                Func<string, string> readParameterName =
                    (elemName) =>
                    taipeiData.Single(o => o.GetProperty("elementName").GetString() == elemName)
                        .GetProperty("time").EnumerateArray().First()
                        .GetProperty("parameter").GetProperty("parameterName").GetString();
                return new WeatherData
                {
                    ZoneName = zoneName,
                    Status = readParameterName("Wx"),
                    MaxTemp = readParameterName("MaxT"),
                    MinTemp = readParameterName("MinT")
                };
            }
        }
    }
}