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


        private string path;
        
        private readonly ILogger<HomeController> _logger;
        IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            path = _appEnvironment.WebRootPath + "/lib/text.docx";
            
        }



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
                try
                {
                    if (cryptData.File != null)
                    {
                        //free license
                        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                        DownloadFile(cryptData);



                        var document = DocumentModel.Load(path);
                        cryptData.EncryptedData = document.Content.ToString();
                        cryptData.DecryptedData = Decryptor.Decode(cryptData.EncryptedData, cryptData.Key);
                        SaveFile(cryptData, true);



                    }
                    else
                    {
                        cryptData.DecryptedData = Decryptor.Decode(cryptData.EncryptedData, cryptData.Key);
                        SaveFile(cryptData, true);

                    }
                    return View(cryptData);
                }
                catch
                {
            
                    return View();
                }

                                    
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
                    try
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
                    catch
                    {
                        return View();
                    }
                    

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
