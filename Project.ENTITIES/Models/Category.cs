using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Category : BaseEntity
    {
        // Kategorinin boş durma ihtimaline göre yarattık projemizi. Ama kategorinin boş durmasını istemiyorsak yani her ürünün kategorisi olsun istiyorsak;
        /*
             public Category()
             (
                 Product = new List<Product>();
             )

        şeklinde tanımlama yapmamız gerekiyor.
         */

        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Relational Properties
        public virtual List<Product> Products { get; set; } // 1'e çok ilişki
    }
}
