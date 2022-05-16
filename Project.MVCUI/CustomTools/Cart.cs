using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.CustomTools

{
    public class Cart
    {
        Dictionary<int, CartItem> _sepetim;
        public Cart()
        {
            _sepetim = new Dictionary<int, CartItem>();  // Ürün atıldığı zaman sepetim oluşuyor.(Cart'dan instance alınıyor)
        }

        public List<CartItem> Sepetim
        {
            get
            {
                return _sepetim.Values.ToList();
            }
        }
        public void SepeteEkle(CartItem item)
        {
            if(_sepetim.ContainsKey(item.ID)) // Sepette aynı üründen bulunup bulunmadığını kontrol ediyoruz..
            {
                _sepetim[item.ID].Amount++;
                return;
            }
            _sepetim.Add(item.ID, item);
        }

        public void SepettenSil(int id)
        {
            if(_sepetim[id].Amount > 1) // 1 den büyükse bir bir eksilt, öyle sepetten çıkar..
            {
                _sepetim[id].Amount--;
                return;
            }
            _sepetim.Remove(id);
        }

        public decimal TotalPrice
        {
            get
            {
                return _sepetim.Sum(x => x.Value.SubTotal);
            }
        }
    }
}