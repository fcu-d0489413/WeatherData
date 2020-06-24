using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherData.Models;

namespace WeatherData.ViewComponents
{
    [Microsoft.AspNetCore.Mvc.ViewComponent]
    public class TaipeiWeatherViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public string Invoke()
        {
            //TODO: 應改用 DI https://blog.darkthread.net/blog/aspnet-core-di-notes/
            var svc = new SimpleWeatherService();
            var data = svc.GetTaipeiWeatherFromOpenDataApi();

            return $"現在台北天氣：{data.Status} /氣溫：{data.MinTemp}° - {data.MaxTemp}°";
        }
    }
}
