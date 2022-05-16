using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
    public class OrderDetailMap : BaseMap<OrderDetail>
    {
        public OrderDetailMap()
        {
            Ignore(x => x.ID);  // Çoka çok tablo olduğu için kendi ID'sini önemsemedik, ilişkiye giren Order ve Product ın ID lerini ekledik.
            HasKey(x => new
            {
                x.OrderID,
                x.ProductID
            });
        }
    }
}
