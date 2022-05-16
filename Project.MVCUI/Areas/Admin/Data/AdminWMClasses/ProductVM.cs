using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Admin.Data.AdminWMClasses
{
    public class ProductVM
    {
        // Listelenen ürünleri sayfalara bölmemiz lazım. Kullandığımız template de Admin panelinde sayfalama güzel bir şekilde yapılmış, oraya dokunmayacağız. Ama alışveriş sisteminde yapılmamış, o yüzden kendimiz  yapacağız. O yüzden 2 tane ProductWM olacak. Birisi Admin için, birde alışverişteki ürünleri listelemek için.

        // PaginationVM ile neredeyse aynı görevi yapıyor gibi gözükebilir. Aslında çok benzer görevleri yapmaktadır. Ancak PaginationVM sadece alışveriş tarafında kullanılacak ve sayfalandırmayı yapacak bir VM iken, ProductVM sadece Admin tarafında kullanılması tasarlanmış bir VM classtır.

        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }

    }
}