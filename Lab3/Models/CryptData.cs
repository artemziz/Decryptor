using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GemBox.Document;


namespace Lab3.Models
{
    public class NotAllowedAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            CryptData val = value as CryptData;
            if (val != null)
            {
                if (val.File == null && (val.DecryptedData == null && val.EncryptedData == null))
                {                   
                    return false;
                }
                else
                {                   
                    return true;
                }
            }
            else
            {
                return false;
            }
            
        }
    }
    [NotAllowedAttribute(ErrorMessage ="Что-то не так")]
    public class CryptData
    {
        public  string EncryptedData { get; set; }

        [Required(ErrorMessage ="Введите ключ")]
        
        public  string Key { get; set; }
        public  string DecryptedData { get; set; }

        public  IFormFile File { get; set; }

        

    }
}
