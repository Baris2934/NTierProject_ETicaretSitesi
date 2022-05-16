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
    public class RegisterController : Controller
    {
        AppUserRepository _apRep;
        ProfileRepository _proRep;

        public RegisterController()
        {
            _apRep = new AppUserRepository();
            _proRep = new ProfileRepository();
        }

        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNow(AppUser appUser, AppUserProfile profile)
        {
            appUser.ActivationCode = Guid.NewGuid(); // Mail için.

            appUser.Password = DantexCrypt.Crypt(appUser.Password); // Şifreyi kriptoladık.

            // Kullanıcı adı ve şifre eşleşmesini sorgulamak için;
            /*
            if(_apRep.Any(x => x.UserName == appUser.UserName) || _apRep.Any(x => x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "Kullanıcı zaten kayıtlı";
                return View();
            }
            */

            if(_apRep.Any(x => x.UserName == appUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanıcı ismi daha önce alınmış";
                return View();
            }
            else if(_apRep.Any(x => x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "Email zaten kayıtlı";
                return View();
            }


            // Kullanıcı başarılı bir şekilde kayıt olduysa önce ona mail gönder:
            // Maildeki aktivasyon kodunu doğru şekilde girerse fake kullanıcı olmadığı anlaşılacak(Guid le benzersiz kod yaratmıştık)
            // Kullanıcının Sitemizdeki Url linkine ulaşması lazım. (MVCUI sağ klik, Properties, Web, Project URL). Mail de onu gönderiyoruz(İstediğimiz sayfaya yönlendirebiliriz)

            string gonderilecekMail = "Tebrikler.. Hesabınız oluşturulmuştur.. Hesabınızı aktive etmek için https://localhost:44366/Register/Activation/" + appUser.ActivationCode + " linkine tıklayabilirsiniz";

            MailService.Send(appUser.Email, body: gonderilecekMail, subject: "Hesap aktivasyon!!");
            _apRep.Add(appUser); // Öncelikle bunu eklemeliyiz. Çünkü AppUser'in ID'si ilk başta oluşmlı.. Çünkü bizim kurduğumuz birebir ilişkide AppUser zorunlu alan, Profile ise opsiyonel alandır. Dolayısıyla ilk başta AppUser'in ID'si SaveChanges ile oluşmalı ki sonra Profile'ı rahatça eklenebilsin..

            if(!string.IsNullOrEmpty(profile.FirstName.Trim()) || !string.IsNullOrEmpty(profile.LastName.Trim()))
            {
                profile.ID = appUser.ID;
                _proRep.Add(profile);
            }

            return View("RegisterOk");
        }

        public ActionResult Activation(Guid id)  // Hesap Aktivasyonu kısmı..
        {
            AppUser aktifEdilecek = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            if(aktifEdilecek != null)
            {
                aktifEdilecek.Active = true;
                _apRep.Update(aktifEdilecek);
                TempData["HesapAktifMi"] = "Hesabınız aktif hale getirildi";
                return RedirectToAction("Login", "Home");
            }

            TempData["HesapAktifMi"] = "Hesabınız ";
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}