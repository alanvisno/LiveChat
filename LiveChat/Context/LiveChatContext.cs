using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Context
{
    public class LiveChatContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=LiveChat_DB;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Encrypt codes
            string EncryptionKey = "LIVECHAT";
            //The stamp, in this case, us an estandar, so it can be reproduce for each user (in this case, adding THE NAME + " - LIVECHAT")
            //Sometimes it can be save in the database but i dont know how to handle that in the correct way
            byte[] variable = Encoding.Unicode.GetBytes("Alan Visnovezky - LIVECHAT");
            byte[] variable2 = Encoding.Unicode.GetBytes("Jesper Simonsen - LIVECHAT");
            string Pass;
            string Pass2;
            byte[] Bytes = Encoding.Unicode.GetBytes("FirstPassword");
            byte[] Bytes2 = Encoding.Unicode.GetBytes("SecondPassword");
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes stamp = new Rfc2898DeriveBytes(EncryptionKey, variable);
                encryptor.Key = stamp.GetBytes(32);
                encryptor.IV = stamp.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(Bytes, 0, Bytes.Length);
                        cs.Close();
                    }
                    Pass = Convert.ToBase64String(ms.ToArray());
                }
            }
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes stamp2 = new Rfc2898DeriveBytes(EncryptionKey, variable2);
                encryptor.Key = stamp2.GetBytes(32);
                encryptor.IV = stamp2.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(Bytes2, 0, Bytes2.Length);
                        cs.Close();
                    }
                    Pass2 = Convert.ToBase64String(ms.ToArray());
                }
            }
            /////////////////////////////////////////////

            var Id = Guid.NewGuid().ToString();
            var Id2 = Guid.NewGuid().ToString();

            //Modifing the table Contact with fluent because it has two foreign keys as principal
            modelBuilder.Entity<Contact>()
                .HasKey(c => new { c.PrincipalId, c.SecondaryId });
            modelBuilder.Entity<Contact>()
                .HasOne(o => o.PrincipalUser)
                .WithMany(m => m.ContactsAsPrincipal)
                .HasForeignKey(f => f.PrincipalId);
            modelBuilder.Entity<Contact>()
                .HasOne(o => o.SecondaryUser)
                .WithMany(m => m.ContactsAsSecondary)
                .HasForeignKey(f => f.SecondaryId);

            //Initialize Data 
            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = Id,
                    Name = "Alan Visnovezky",
                    Password = Pass,
                    Image = "https://www.lifebonder.com/imagelink"
                }, new User
                {
                    Id = Id2,
                    Name = "Jesper Simonsen",
                    Password = Pass2,
                    Image = "https://www.lifebonder.com/imagelink"
                });
            modelBuilder.Entity<Contact>()
                .HasData(new Contact
                {
                    PrincipalId = Id,
                    SecondaryId = Id2,
                    Message = null
                });
        }

        public DbSet<User> User { get; set; }
        public DbSet<Contact> Contact { get; set; }

        public DbSet<Message> Message { get; set; }
    }
}
