using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChat.Model
{
    public class MessageModel
    {
        //Model for the API messages
        [Required]
        public string userid { get; set; }

        [Required]
        public string recipientid { get; set; }

        [Required]
        public string securitycode { get; set; }

        [Required]
        public string message { get; set; }
    }
}
