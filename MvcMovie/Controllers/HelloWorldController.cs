using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{

    public class HelloWorldController : Controller
    {

        public IActionResult Index()
        {
            Console.WriteLine("abc");
            Console.WriteLine("abc");
            return View();
        }

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            Console.WriteLine("abc");
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}
