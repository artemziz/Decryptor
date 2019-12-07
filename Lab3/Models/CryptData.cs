using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models
{
    public class CryptData
    {
        public  string EncryptedData { get; set; }
        public  string Key { get; set; }
        public  string DecryptedData { get; set; }

        public  IFormFile File { get; set; }
    }
}
