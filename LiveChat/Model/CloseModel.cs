using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChat.Model
{
    public class CloseModel
    {
        [Required]
        public string userid { get; set; }
    }
}
