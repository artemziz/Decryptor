using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab3.Models;

namespace Lab3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Decrypt()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Decrypt(CryptData cryptData)
        {
            cryptData.DecryptedData = "Расшифровано";
            return View(cryptData);
        }


        [HttpGet]    
        public IActionResult Encrypt()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Encrypt(CryptData cryptData)
        {
            cryptData.EncryptedData = "Зашифровано";
            return View(cryptData);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
