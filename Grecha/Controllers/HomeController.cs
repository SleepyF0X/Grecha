using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grecha.Parsing;
using Microsoft.Extensions.Logging;

namespace Grecha.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            AtbParser parser = new AtbParser();
            var out1 = parser.Parse("гречана");
            Console.Out.Write(out1);
            return View();
        }
    }
}
