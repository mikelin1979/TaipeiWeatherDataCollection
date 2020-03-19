using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PWEBTaipei.Bussiness;
using PWEBTaipei.Models;

namespace PWEBTaipei.Controllers
{
    public class HomeController : Controller
    {
        private PWEBAgent agent = new PWEBAgent();
        public IActionResult Index()
        {
            //var data = agent.get("北投區");
            return View();
        }

        [HttpPost]
        public JsonResult Query([FromBody] QueryModel input)
        {
            LineChartViewModel result = new LineChartViewModel();

            result = agent.get(input.location, input.sttime, input.edtime);
            return Json(result);
        }
    }
}
