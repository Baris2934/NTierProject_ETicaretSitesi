using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.AuthenticationClasses
{
    public class AdminAuthentication : AuthorizeAttribute
    { 
        // Özel kimlik kontrollerini gerçekleştirebileceğimiz attribute; 

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Metodun içinde kullanıcımın giriş yapıp yapmadığını kontrol ederek true veya false dönüyorum. Bunun anlamı eğer true dönerse yetki onaylanmış, false dönerse yetki onaylanmamıştır. True dönmesi durumunda attribute‘u kullandığımız action çalışacak ve işlem devam edecek. Ancak yetki onaylanmadıysa ve false döndüyse kullanıcıyı giriş sayfasına yönlendirmek istiyoruz.

            if (httpContext.Session["admin"] != null)   
            {
                return true;
            }
            httpContext.Response.Redirect("/Home/Login");
            return false; 
        }
    }
}