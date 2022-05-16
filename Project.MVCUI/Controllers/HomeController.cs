using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        // Login işlemleri burada yapılacaktır.

        AppUserRepository _apRep;

        public HomeController()
        {
            _apRep = new AppUserRepository();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(AppUser appUser)
        {
            // İlk önce kullanıcı adı üzerinden sorgulama yapıyoruz.

            AppUser yakalanan = _apRep.FirstOrDefault(x => x.UserName == appUser.UserName);

            if(yakalanan == null)
            {
                ViewBag.Kullanici = "Kullanıcı bulunamadı";
                return View();
            }

            // Burası tamamlanırsa, şifresini dantexcrypt'de sakladığımız için decrypted(çözmemiz) yapmamız gerekiyor..

            string decrypted = DantexCrypt.DeCrypt(yakalanan.Password);

            if(appUser.Password == decrypted && yakalanan.Role == ENTITIES.Enums.UserRole.Admin)
            {
                if (!yakalanan.Active) return AktifKontrol();

                Session["admin"] = yakalanan;
                return RedirectToAction("CategoryList", "Category", new { area = "Admin" });
            }
            else if(appUser.Password == decrypted && yakalanan.Role == ENTITIES.Enums.UserRole.Member)
            {
                if (!yakalanan.Active) return AktifKontrol();
                Session["member"] = yakalanan;
                return RedirectToAction("ShoppingList", "Shopping");
            }

            ViewBag.Kullanici = "Kullanıcı bulunamadı";
            return View();
        }

        private ActionResult AktifKontrol()
        {
            ViewBag.Kullanici = "Lütfen hesabınızı aktif hale getiriniz";
            return View("Login");
        }
    }
}