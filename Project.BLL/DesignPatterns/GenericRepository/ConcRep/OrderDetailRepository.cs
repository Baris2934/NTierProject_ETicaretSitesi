using Project.BLL.DesignPatterns.GenericRepository.BaseRep;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.GenericRepository.ConcRep
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>
    {
        // Order Detail tablosu, çoka çok ilşki sonucu oluşturulmuş bir tablodur. BaseRepository'deki Update işlemi, ID üzerinden gerçekleştiriliyor. Fakat Order Detail tablosunun iki tane ID'si(Primary Key'i) var. Biz gidip tek bir ID'den yakalamaya çalışırsak, bize bu ID'de uniq bir şey yok diyecek. O yüzden BaseRepository'deki Update işlemi(ID'den dolayı) burada çalışmayacaktır. Bunu daha önce tahmin edebildiğimiz için Update metodunu virtual olarak işaretlemiştik. O yüzden burada override ederek, bu class'ın kullanabileceği duruma göre değiştireceğiz.

        public override void Update(OrderDetail item)
        {
            OrderDetail toBeUpdated = FirstOrDefault(x => x.ProductID == item.ProductID && x.OrderID == item.OrderID); // ID'leri eşleştirdik.
            item.Status = ENTITIES.Enums.DataStatus.Updated;
            item.ModifiedDate = DateTime.Now;
            _db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            Save();
        }

        // Update için yaptığımız olayı, UpdateRange için de yapıyoruz; 
        public override void UpdateRange(List<OrderDetail> list)
        {
            foreach (OrderDetail item in list)
            {
                Update(item); // Üst taraftaki Update metodu.
            }
        }
    }
}
