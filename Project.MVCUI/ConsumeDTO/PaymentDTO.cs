using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.ConsumeDTO
{
    public class PaymentDTO
    {
        // Sanal Pos Entegrasyonu yapmak istediğimiz de normal şartlarda bankanın bize dökümantasyon göndermesi lazım ve ona göre yazmamız lazım. Ancak bizde şu an banka bilgileri olmadığı için kendimiz basit bir BankaAPI projesi yazıp onun üzerinden işlem yapacağız.

        public int ID { get; set; }
        public string CardUserName { get; set; }
        public string SecurityNumber { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public decimal ShoppingPrice { get; set; }
    }
}