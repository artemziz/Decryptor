using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab3.Models;
using GemBox.Document;
using System.IO;

namespace Lab3.Controllers
{
    public static class FileManager
    {
        public static void DownloadFile(CryptData cryptData, string path)
        {
            //free license
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            using (FileStream fs = System.IO.File.Create(path))
            {
                cryptData.File.CopyTo(fs);
            }
        }

        public static void SaveFile(CryptData cryptData, string path, bool isDecrypted)
        {
            //free license
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var doc = new DocumentModel();

            if (isDecrypted)
            {
                doc.Content.LoadText(cryptData.DecryptedData);
            }
            else
            {
                doc.Content.LoadText(cryptData.EncryptedData);
            }

            doc.Save(path);
        }
    }
}
