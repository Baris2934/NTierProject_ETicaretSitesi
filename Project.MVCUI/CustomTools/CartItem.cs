using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.CustomTools
{
    public class CartItem
    {
        // Sepetimize atacağımız ürün temsili bilgiler..

        public int ID { get; set; }
        public string Name { get; set; } // Adı
        public short Amount { get; set; } // Miktar
        public decimal Price { get; set; } // Fiyat
        public string ImagePath { get; set; } // Resmi
        public decimal SubTotal // Toplam
        {
            get
            {
                return Price * Amount;
            }
        }
        public CartItem()
        {
            Amount++;
        }
    }
}