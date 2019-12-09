using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Controllers
{
    public static class Decryptor
    {
        static private readonly char[] alpabet = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я','0','1','2','3','4','5','6','7','8','9' };

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
}
