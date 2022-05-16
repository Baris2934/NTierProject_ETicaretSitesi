using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class AppUser : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid ActivationCode { get; set; } // Guid; taklit edilemeyen, benzersiz bir şifreleme oluşturmaya yarıyor. AppUser'a gönderilen aktivasyon kodu sadece ona has olmasını istiyoruz.
        public bool Active { get; set; }
        public string Email { get; set; } // Kullanıcının Email'i olacak
        public UserRole Role { get; set; } // Kullanıcının UserRole'ü olacak. Bunu da constructor da tanımlayacağız.

        public AppUser()
        {
            Role = UserRole.Member; 
        }

        // Relational Properties
        public virtual AppUserProfile Profile { get; set; }
        public virtual List<Order> Orders { get; set; }

    }
}
