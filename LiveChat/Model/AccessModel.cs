using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChat.Model
{
    public class AccessModel
    {
        [Required]
        public string userid { get; set; }

        [Required]
        public string password { get; set; }
    }
}
