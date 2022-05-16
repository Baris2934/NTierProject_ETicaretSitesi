using PagedList;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.CustomTools;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {
        OrderRepository _oRep;
        ProductRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;

        public ShoppingController()
        {
            _pRep = new ProductRepository();
            _odRep = new OrderDetailRepository();
            _oRep = new OrderRepository();
            _cRep = new CategoryRepository();
        }

        /*
         string a = "Mehmet";
         string b = a ?? "Ali";

        Yani burada demek istediğimiz; a null ise b nin değerine Ali yi at demek, ama a null değilse de b nin değerine a yı(Mehmet) at demektir.
         
         */


        
        public ActionResult ShoppingList(int? page, int? categoryID) // nullable i vermemizin sebebi, aslında buradaki int'in kaçıncı sayfada oludğumuzu temsil edecek olmasıdır. Ancak birisi direkt alışveriş sayfasına ulaştığında hangi sayfada oluduğu verisi olamayacağından dolayı bu şekilde de (yani sayfa numarası gönderilmeden de) nu Action'ın çalışabilmesini istiyoruz..
        {
            // page?? => ifadesi page null ise demektir.

            PaginationVM pavm = new PaginationVM()
            {
                PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9), //page null ise 1. sayfadan başlat, değilse kaçsa oradan başlat. bir sayfada da 9 ürün olsun demek istedik.
                Categories = _cRep.GetActives()
            };

            // Hangi kategoride olduğumu bilgisayarın aklında tutmak için;
            if (categoryID != null) TempData["catID"] = categoryID;  // Herhangi bir kategori seçtik. Bulunacağımız sayfadaki 9 ürün o kategoriden gelecek. Ama başka sayfaya geçtiğimde hangi kategoride bulunduğumu unutacağı için bütün kategorideki ürünleri listelemeye başlayacaktır. O yüzden geçtiğim o sayfaya, seçtiğim kategoride olduğumu bildirmek için TempData kullandık.


            return View(pavm);
        }

        public ActionResult AddToCart(int id) 
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;  // Sepeti yakalamamız lazım. Eğer Session boşsa yeni bir Cart yaratacak, boş değilse mevcut Session'ı kullanacak. (Session object tipte değer tutar.)
            Product eklenecekUrun = _pRep.Find(id);  // Hangi ürünün ekleneceğini buluyoruz..

            CartItem ci = new CartItem   // Yakaladığımız eklenecek ürünün bilgilerini alacak..
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };

            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }

        public ActionResult CartPage()
        {
            if(Session["scart"] != null)
            {
                CartPageVM cpvm = new CartPageVM();
                Cart c = Session["scart"] as Cart;
                cpvm.Cart = c;
                return View(cpvm);
            }

            TempData["bos"] = "Sepetinizde ürün bulunmamaktadır.";
            return RedirectToAction("ShoppingList");
        }

        public ActionResult DeleteFromCart(int id)
        {
            if(Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenSil(id);
                if(c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadır.";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }
            return RedirectToAction("ShoppingList");
        }

        public ActionResult SiparisiOnayla()
        {
            AppUser mevcutKullanici;
            if (Session["member"] != null)
            {
                mevcutKullanici = Session["member"] as AppUser;
            }
            else TempData["anonim"] = "Kullanıcı üye değil";
            return View();
        }

        // https://localhost:44312/api/Payment/ReceivePayment
        // WebApiRestService.WebApiClient kütüphanesini indirmeyi unutmayın çünkü API ile bu kütüphane sayesinde BackEnd'in haberleşmesini sağlayacağız...

        [HttpPost]
        public ActionResult SiparisiOnayla(OrderVM ovm)
        {
            bool result;
            Cart sepet = Session["scart"] as Cart;

            if (Session["member"] != null)
            {
                AppUser kullanici = Session["member"] as AppUser;
                ovm.Order.Email = kullanici.Email;
                ovm.Order.UserName = kullanici.UserName;
                ovm.Order.AppUserID = kullanici.ID;
            }
            else ovm.Order.UserName = TempData["anonim"].ToString();

            ovm.Order.TotalPrice = ovm.PaymentDTO.ShoppingPrice = sepet.TotalPrice;


            #region APISection  
            // API ile haberleşiyoruz.. (BackEnd'den API'a istek yaparız(client), API'da bie bir cevap döndürür(Bu cevaplar BackEnd'de her zaman Task tipinde tutulurlar.) )

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44312/api/");  // İstek yapacağımız temel adres

                // BackEnd'den bir istek yaptığımızda her zaman bu API isteklerini Task ile yaparız. Task, bir görev tipi demektir.(Görev yaptıran bir tip)
                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync("Payment/ReceivePayment",ovm.PaymentDTO);

                HttpResponseMessage sonuc;

                try
                {
                    sonuc = postTask.Result;
                }
                catch (Exception)
                {

                    TempData["baglantiRed"] = "Banka bağlantıyı reddetti";
                    return RedirectToAction("ShoppingList");
                }

                if (sonuc.IsSuccessStatusCode) result = true;
                else result = false;
                if(result)
                {
                    _oRep.Add(ovm.Order);  // OrderRepository bu noktada Order'ı eklerken onun ID'sini oluşturuyor.

                    foreach(CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _odRep.Add(od);

                        // Ücret ödendikten sonra Stoktan da düşürelim;
                        Product stokDus = _pRep.Find(item.ID);
                        stokDus.UnitInStock -= item.Amount;
                        _pRep.Update(stokDus);
                    }

                    TempData["odeme"] = "Siparişiniz bize ulaşmıştır..Teşekkür ederiz..";
                    MailService.Send(ovm.Order.Email, body: $"Siparişiniz başarı ile alındı...{ovm.Order.TotalPrice}");
                    Session.Remove("scart"); // Sipariş tamamlandıktan sonra sepeti boşalt..
                    return RedirectToAction("ShoppingList");
                }
                else
                {
                    TempData["sorun"] = "Ödeme ile ilgili bir sorun oluştu. Lütfen bankanız ile iletişime geçiniz..";
                    return RedirectToAction("ShoppingList");
                }

            }

            #endregion


            
        }
    }
}