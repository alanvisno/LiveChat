using LiveChat.Context;
using LiveChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using LiveChat.Hubs;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Services
{
    public class LiveChatServices
    {
        ChatHub chathub = ChatHub.Instance;

        //SINGLETON PATTERN (to use the same instance for each call)
        private readonly static LiveChatServices _instance = new LiveChatServices();

        private LiveChatServices()
        {

        }

        public static LiveChatServices Instance
        {
            get
            {
                return _instance;
            }
        }
        ///////////////////////////////

        public string GetAccess(AccessModel model)
        {
            using (var _context = new LiveChatContext())
            {
                //Search the principal user
                var principal = _context.User.Find(model.userid);
                if (principal == null)
                {
                    return null;
                }

                //Encript password and compare it with the one in the DB (the one way encryption provides a single way to check the password, so it can be safe)
                byte[] variable = Encoding.Unicode.GetBytes(principal.Name + " - LIVECHAT");
                byte[] Bytes = Encoding.Unicode.GetBytes(model.password);
                string Pass = this.EncryptPassword(variable, Bytes);
                if (Pass != principal.Password)
                {
                    return null;
                }

                string token = Guid.NewGuid().ToString();
                principal.Token = token;
                _context.SaveChanges();

                return token;
            }
        }

        public List<ContactDataModel> GetContacts(ContactModel model)
        {
            using (var _context = new LiveChatContext())
            {
                //Search the principal user
                var principal = _context.User.Find(model.userid);
                if (principal == null)
                {
                    return null;
                }

                //Check token
                if (model.securitycode != principal.Token)
                {
                    return null;
                }

                var lista = _context.Contact
                    .Where(x => x.PrincipalId == model.userid)
                    .Include(x => x.SecondaryUser)
                    .ToList();

                //Send a different data model to avoid another information
                var newl = new List<ContactDataModel>();
                foreach (var item in lista)
                {
                    var data = new ContactDataModel();
                    data.Message = item.Message;
                    data.PrincipalId = item.PrincipalId;
                    data.SecondaryId = item.SecondaryId;
                    data.PrincipalName = item.PrincipalUser.Name;
                    data.SecondaryName = item.SecondaryUser.Name;
                    newl.Add(data);
                }
                return newl;
            }
        }

        public async Task<List<Message>> GetMessages(MessageModel model)
        {
            //"Using" statment for dispose the model at the end
            using (var _context = new LiveChatContext())
            {
                //Search the principal user
                var principal = _context.User.Find(model.userid);
                if (principal == null)
                {
                    return null;
                }

                //Check token
                if (model.securitycode != principal.Token)
                {
                    return null;
                }

                //Search the recipient
                var secondary = _context.User.Find(model.recipientid);
                if (secondary == null)
                {
                    return null;
                }

                //Set first message (async task)
                this.SetMessageFirst(_context, principal, secondary, model.message);
                this.SetMessage(_context, model);
                //await chathub.Clients.All.SendAsync("Receive", model.userid, model.message);

                return await _context.Message
                    .Where(x => x.PrincipalId == principal.Id && x.SecondaryId == secondary.Id)
                    .ToListAsync();
            }
        }

        public async Task<bool> CheckUser(MessageModel model)
        {
            try
            {
                //"Using" statment for dispose the model at the end
                using (var _context = new LiveChatContext())
                {
                    //Search the principal user
                    var principal = _context.User.Find(model.userid);
                    if (principal == null)
                    {
                        return false;
                    }

                    //Check token
                    if (model.securitycode != principal.Token)
                    {
                        return false;
                    }

                    //Search the recipient
                    var secondary = _context.User.Find(model.recipientid);
                    if (secondary == null)
                    {
                        return false;
                    }

                    //Set first message (async task)
                    this.SetMessageFirst(_context, principal, secondary, model.message);
                    this.SetMessage(_context, model);
                    //await chathub.Clients.All.SendAsync("Receive", model.userid, model.message);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string EncryptPassword(byte[] variable, byte[] Bytes)
        {
            using (Aes encryptor = Aes.Create())
            {
                string EncryptionKey = "LIVECHAT";
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
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private void SetMessageFirst(LiveChatContext _context, User principal, User secondary, string message)
        {
            var contact = _context.Contact.Find(principal.Id, secondary.Id);
            //If the first message is null, create it
            if (contact.Message == null)
            {
                contact.Message = message;
                _context.SaveChanges();
            }
        }
        private void SetMessage(LiveChatContext _context, MessageModel model)
        {
            var message = new Message();
            //Set type 0 (text message) as the task didnt discribe the task
            message.Type = 0;
            message.PrincipalId = model.userid;
            message.SecondaryId = model.recipientid;
            message.String = model.message;
            message.Datetime = DateTime.Now;
            _context.Add(message);
            _context.SaveChanges();
        }

        public bool CloseAccess(CloseModel model)
        {
            using (var _context = new LiveChatContext())
            {
                //Search the principal user
                var principal = _context.User.Find(model.userid);
                if (principal == null)
                {
                    return false;
                }

                //Delete token
                principal.Token = null;
                _context.SaveChanges();

                return true;
            }
        }
    }
}
