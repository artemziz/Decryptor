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
    public static class Decryptor
    {
        static private readonly char[] alpabet = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

        static public string Decode(string file, string key)
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

        static public string Encode(string file, string key)
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
    }
    public class HomeController : Controller
    {


        private string path;
        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;

        

        public void DownloadFile(CryptData cryptData)
        {
            using (FileStream fs = System.IO.File.Create(path))
            {
                cryptData.File.CopyTo(fs);
            }
        }

        public void SaveFile(CryptData cryptData,bool answer)
        {
            //free license
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var doc = new DocumentModel();
                           
                if (answer)
                {
                    doc.Content.LoadText(cryptData.DecryptedData);
                }
                else
                {
                    doc.Content.LoadText(cryptData.EncryptedData);
                }
                
            doc.Save(path);
        }
        

        
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
                    

                    if (cryptData.File != null)
                    {
                        //free license
                        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                        DownloadFile(cryptData);



                        var document = DocumentModel.Load(path);
                        cryptData.EncryptedData = document.Content.ToString();
                        cryptData.DecryptedData = Decryptor.Decode(cryptData.EncryptedData, cryptData.Key);
                        SaveFile(cryptData,true);
                        


                    }
                    else
                    {
                        cryptData.DecryptedData = Decryptor.Decode(cryptData.EncryptedData, cryptData.Key);
                        SaveFile(cryptData,true);

                    }
                    return View(cryptData);                 
            }
            else
            {
                return View();
            }
            
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
                
                if (cryptData.File != null)
                {
                    //free license
                    ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                    DownloadFile(cryptData);

                    // Load Word document from file's path.
                    var document = DocumentModel.Load(path);
                    cryptData.DecryptedData = document.Content.ToString();

                    System.IO.File.Delete(path);
                    cryptData.EncryptedData = Decryptor.Encode(cryptData.DecryptedData, cryptData.Key);
                    SaveFile(cryptData, false);

                }
                else
                {
                    cryptData.EncryptedData = Decryptor.Encode(cryptData.DecryptedData, cryptData.Key);
                    SaveFile(cryptData, false);

                }
                return View(cryptData);
            }
            else
            {
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult Download()
        {
            return PhysicalFile(path, "text/plain", "Result.docx");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
