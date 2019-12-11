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
                        FileManager.DownloadFile(cryptData,path);

                        //Work with Word File
                        var document = DocumentModel.Load(path);
                        cryptData.EncryptedData = document.Content.ToString();
                        cryptData.DecryptedData = Decryptor.Decode(cryptData.EncryptedData, cryptData.Key);

                        //Saving result
                        FileManager.SaveFile(cryptData,path, true);

                    }
                    else
                    {
                        //Decode from Text
                        cryptData.DecryptedData = Decryptor.Decode(cryptData.EncryptedData, cryptData.Key);
                        FileManager.SaveFile(cryptData,path, true);
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
                try 
                { 
                    if (cryptData.File != null)
                    {
                                         
                            FileManager.DownloadFile(cryptData,path);

                            //Work with Word File

                            var document = DocumentModel.Load(path);
                            cryptData.DecryptedData = document.Content.ToString();                           
                            cryptData.EncryptedData = Decryptor.Encode(cryptData.DecryptedData, cryptData.Key);
                            //Saving result
                            FileManager.SaveFile(cryptData,path, false);                   
                    
                    }
                    else
                    {
                        //Decode from Text
                        cryptData.EncryptedData = Decryptor.Encode(cryptData.DecryptedData, cryptData.Key);
                        FileManager.SaveFile(cryptData, path, false);

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
        public ActionResult Download()
        {
            return PhysicalFile(path, "text/plain", "Result.docx");
        }

       
    }
}
