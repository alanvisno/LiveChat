using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChat.Context
{
    public class User
    {
        public string Name { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Password { get; set; }

        public string Image { get; set; }

        public string Token { get; set; }

        public virtual ICollection<Contact> ContactsAsPrincipal { get; set; }
        public virtual ICollection<Contact> ContactsAsSecondary { get; set; }
    }
}
