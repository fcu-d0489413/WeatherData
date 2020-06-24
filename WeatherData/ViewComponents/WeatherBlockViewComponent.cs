using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherData.Models;

namespace WeatherData.ViewComponents
{
    public class WeatherBlockViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string zoneName)
        {
            var svc = new SimpleWeatherService();
            var data = svc.GetWeatherFromOpenDataApi(zoneName);
            return View(data);
        }
    }
}
