using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab3.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

using GemBox.Document;

namespace Lab3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
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
        public  IActionResult Encrypt(CryptData cryptData)
        {
            //free license
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");


            if (cryptData.File !=null)
            {
                string path = _appEnvironment.WebRootPath+"/files/text.docx";
                using(FileStream fs = System.IO.File.Create(path))
                {
                    cryptData.File.CopyTo(fs);
                    
                }
                // Load Word document from file's path.
                var document = DocumentModel.Load(path);
                cryptData.DecryptedData = document.Content.ToString();
                System.IO.File.Delete(path);













            }


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
