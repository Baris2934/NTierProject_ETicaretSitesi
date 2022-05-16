using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.COMMON.Tools
{
    public static class DantexCrypt
    {
     
        public static string Crypt(string a)
        {
            Random rnd = new Random();
            char[] charArray = { '*', '_', '?' };  // 3 char'dan oluşan bir char array'i var.
            string hashedCode = ""; // hasdhedcode'u öncelikle boş başlatıyor. Bu bizim geriye değer döndüreceğimiz kod olacak.
            foreach (char item in a)
            {
                int tempInteger; // foreach'ın her turunda tempInteger oluşuyor.
                switch (rnd.Next(1, 4))  // 1'le 4 arasında dönüyoruz(4 dahil)
                {
                    case 1: // Eğer ramdom'dan 1  gelirse;
                        tempInteger = (Convert.ToInt32(item) + 1) * 2; // Onun ASCII kodunu 1 ile toplayıp, 2 ile çarpıyoruz.
                        hashedCode += $"{tempInteger}{charArray[0]}"; // hashedcode'a gidip Ascıı olan tempInteger'ımı string'e çevirip, charArray'den de ' * ' ı ekliyorum.
                        break;
                    case 2:
                        tempInteger = (Convert.ToInt32(item) + 2) * 3;
                        hashedCode += $"{tempInteger}{charArray[1]}";
                        break;
                    case 3:
                        tempInteger = (Convert.ToInt32(item) + 3) * 4;
                        hashedCode += $"{tempInteger}{charArray[2]}";
                        break;
                }

            }

            return hashedCode;
        }

        public static string DeCrypt(string a)
        {
            string decryptedCode = "";  
            

            // Crypt'de her case'de array'e bir karakter ekledik. Daha sonra Regex'le bunları ayırmaya çalıştık.
            // Regex, bize gelen bir cümleyi ayırıyor. 
            List<string> parts = Regex.Split(a, @"(?<=[*_?])").ToList(); //split gibi iş yapar anca *_? karakterlerini de alır..



            foreach (string item in parts) // ayırdığımız elemanları bir listeye atıyoruz ve o listede dönüyoruz.
            {

                if (item.Contains("*")) // Eğer * içeriyorsa ; 
                {

                    string element = item.TrimEnd('*'); // * 'ı çıkar; 

                    int asciiCode = (Convert.ToInt32(element) / 2) - 1; // 2'ye böl, 1 çıkar;
                    string character = Convert.ToChar(asciiCode).ToString(); 
                    decryptedCode += character;
                }

                else if (item.Contains("_"))
                {
                    string element2 = item.TrimEnd('_');
                    int asciiCode2 = (Convert.ToInt32(element2) / 3) - 2;
                    string character2 = Convert.ToChar(asciiCode2).ToString();
                    decryptedCode += character2;
                }

                else if (item.Contains("?"))
                {

                    string element3 = item.TrimEnd('?');
                    int asciiCode3 = (Convert.ToInt32(element3) / 4) - 3;
                    string character3 = Convert.ToChar(asciiCode3).ToString();
                    decryptedCode += character3;
                }






            }
            return decryptedCode;


            //string[] saltArray = a.Split('*','_','?');
            //foreach (string item in saltArray)
            //{
            //    int asciiCode = Convert.ToInt32(item);
            //    int tempChar = (asciiCode / 3) - 3;
            //    string character = Convert.ToChar(tempChar).ToString();
            //    decryptedCode += character;
            //}
            //return decryptedCode;
        }
    }
}
