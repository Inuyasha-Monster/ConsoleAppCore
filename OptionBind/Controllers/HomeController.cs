using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OptionBind.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IOptions<ClassTest> _option;

        //public HomeController(IOptions<ClassTest> _option)
        //{
        //    this._option = _option;
        //}

        // GET: /<controller>/
        public IActionResult Index()
        {
            //return View(_option.Value);
            return View();
        }
    }
}
