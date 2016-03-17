using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SimpleInjectorCore.Core;

namespace SimpleInjectorCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPessoaService _pessoaService;

        public HomeController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        public IActionResult Index()
        {
            var model = _pessoaService.Listar();
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
