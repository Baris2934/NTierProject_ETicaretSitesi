using Bogus.DataSets;
using Project.COMMON.Tools;
using Project.DAL.Context;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.StrategyPattern
{
    // Bogus kütüphanesi, bize hazır Data sunan bir kütüphanedir. Bunu DAL katmanına indiriyoruz. 

    // Bogus, FakeData olarak karşımıza çıkıyor. (Bir ticari ortamda ne gibi isimler çıkarsa onları söylüyor bize. Kategori ismi, ürün ismi vb.) Fakat şöyle bir dezavantajı var. Mesela Türk isminde isimler oluştururken çok saçma isimler yaratabiliyor. Bu yüzden Seed metodunu override(ezmek) ederek isteğimizi karşılayabiliyoruz.
    public class MyInit : CreateDatabaseIfNotExists<MyContext>
    {

        // Seed metodu; bir veri tabanı oluşturulurken, o oluşturulan model içerisine bilgilerin girilmesini sağlıyor. Veritabanının bilgilerle, verilerle oluşmasını sağlıyor.
        protected override void Seed(MyContext context)
        {
            #region Admin

            AppUser au = new AppUser(); // Kullanıcı oluşturuyoruz.(Admin ekliyoruz)
            au.UserName = "brs";
            au.Password = DantexCrypt.Crypt("123"); // Şifreleme sınıfımız Common katmanında olduğu için DAL'a COMMON'dan referans veriyoruz. Şifreleme sınıfımız yardımı ile şifre oluşturuyoruz. Veritabanında şifre açık şekilde görünmemiş oluyor.
            au.Email = "bariscelik@gmail.com";
            au.Role = ENTITIES.Enums.UserRole.Admin;
            context.AppUsers.Add(au);
            context.SaveChanges();

            #endregion

            // 1 tane Admin ekleyince identity otomatik olarak ID yi 1 yaptı. Bundan sonra ekleyeceğim bütün kullanıcılar 2 den başlayacak. Kaç tane kullanıcı istiyorsak döngü ile döneriz.

            #region NormalUsers 

            // FakeData oluşturacağız. 10 tane kullanıcı eklemek istiyoruz.

            for (int i = 0; i < 10; i++)
            {
                AppUser ap = new AppUser();
                ap.UserName = new Internet("tr").UserName(); // Bogus'dan aldık. Internet Bogus a ait bir sınıf. Fake bilgiler.
                ap.Password = new Internet("tr").Password();
                ap.Email = new Internet("tr").Email();
                context.AppUsers.Add(ap);
            }

            context.SaveChanges(); // kullanıcılarımızı oluşturduk.

            // Şimdi her kullanıcıya denk düşecek bir profil oluşturacağız. Birinci ID Admin'e ait olduğu için 2 den başlıyoruz ve 10 adet kullanıcıya profil oluşturuyoruz.
            // Tekrar döngü açıp;

            for (int i = 2; i < 12; i++)
            {
                AppUserProfile aup = new AppUserProfile();
                aup.ID = i; // Birebir ilişki olduğundan dolayı üst tarafta oluşturulan AppUser'ların ID'leri ile bunlar eşleşmeli. O yüzden döngünün iterasyonunu 2'den başlattık.
                aup.FirstName = new Name("tr").FirstName();
                aup.LastName = new Name("tr").LastName();
                context.Profiles.Add(aup);
            }

            context.SaveChanges();

            #endregion


            #region Categories-Products

            // Şimdi 10 tane Kategori yaratmak istiyoruz ve her kategoride de 30'ar tane ürün olmasını istiyoruz. Toplam 300 tane ürünüm olmasını istiyoruz. Yine Bogus kullancağız.

            for (int i = 0; i < 10; i++)
            {
                Category c = new Category();
                c.CategoryName = new Commerce("tr").Categories(1)[0]; // Categories string array döndürüyor. 1 elemanlı bir array döndür dedik.
                c.Description = new Lorem("tr").Sentence(10); // 10 cümlelik kelimler topluluğu oluşturduk, Description boş kalmasın diye.
                c.Products = new List<Product>();

                // Bir kategori yaratldığı anda ona hemen 30 tane ürün ekleyelim;

                for (int j = 0; j < 30; j++)
                {
                    Product p = new Product();
                    p.ProductName = new Commerce("tr").ProductName();
                    p.UnitPrice = Convert.ToDecimal(new Commerce("tr").Price());
                    p.UnitInStock = 100;
                    p.ImagePath = new Images().Nightlife();
                    c.Products.Add(p);
                }

                context.Categories.Add(c);
                context.SaveChanges();
            }

            #endregion


        }
    }
}
