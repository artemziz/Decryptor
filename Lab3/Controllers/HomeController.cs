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


        private readonly char[] alpabet = { 'а','б','в','г','д','е','ё','ж','з','и','й','к','л','м','н','о','п','р','с','т','у','ф','х','ц','ч','ш','щ','ъ','ы','ь','э','ю','я' };
        private string path;

        public void DownloadFile(CryptData cryptData)
        {
            using (FileStream fs = System.IO.File.Create(path))
            {
                cryptData.File.CopyTo(fs);
            }
        }

        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            path = _appEnvironment.WebRootPath + "/lib/text.docx";
        }

        public IActionResult Index()
        {
            return View();
        }

        public string Decode(string file,string key)
        {
            key = key.ToLower();
            string result = "";
            int keyword_index = 0;
            foreach (char symbol in file)
            {
                if (alpabet.Contains(symbol))
                {
                    int p = (Array.IndexOf(alpabet, symbol) + alpabet.Length - Array.IndexOf(alpabet, key[keyword_index])) % alpabet.Length;
                    result += alpabet[p];

                    if ((keyword_index + 1) != key.Length)
                    {
                        keyword_index++;
                    }
                    else
                    {
                        keyword_index = 0;
                    }
                        
                }
                else
                {
                    result += symbol.ToString();   
                }
            }

            return result;
        }
        [HttpGet]
        public IActionResult Decrypt()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Decrypt(CryptData cryptData)
        {
            if (ModelState.IsValid)
            {
                //free license
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");

                if (cryptData.File != null)
                {
                    DownloadFile(cryptData);


                    var document = DocumentModel.Load(path);
                    cryptData.EncryptedData = document.Content.ToString();
                    System.IO.File.Delete(path);
                    cryptData.DecryptedData = Decode(cryptData.EncryptedData, cryptData.Key);
                }
                else
                {
                    cryptData.DecryptedData = Decode(cryptData.EncryptedData, cryptData.Key);
                }
                return View(cryptData);
            }
            else
            {
                return View();
            }
            
        }

        public string Encode(string file,string key) 
        { 
            
            string result = "";
            key = key.ToLower();
            int keyword_index = 0;
            foreach (char symbol in file)
            {
                if (alpabet.Contains(symbol))
                {
                    int c = (Array.IndexOf(alpabet, symbol) + Array.IndexOf(alpabet, key[keyword_index])) % alpabet.Length;

                    result += alpabet[c];

                    if ((keyword_index + 1) != key.Length)
                    {
                        keyword_index++;
                    }
                    else
                    {
                        keyword_index = 0;
                    }
                }
                else
                {
                    result += symbol;
                   
                }
            }
            return result;
        }

        [HttpGet]    
        public IActionResult Encrypt()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult Encrypt(CryptData cryptData)
        {
            if (ModelState.IsValid)
            {
                //free license
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");

                if (cryptData.File != null)
                {

                    DownloadFile(cryptData);

                    // Load Word document from file's path.
                    var document = DocumentModel.Load(path);
                    cryptData.DecryptedData = document.Content.ToString();

                    System.IO.File.Delete(path);
                    cryptData.EncryptedData = Encode(cryptData.DecryptedData, cryptData.Key);
                }
                else
                {
                    cryptData.EncryptedData = Encode(cryptData.DecryptedData, cryptData.Key);
                }
                return View(cryptData);
            }
            else
            {
                return View();
            }
            
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
