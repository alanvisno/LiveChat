using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LiveChat.Context
{
    public class Contact
    {
        //There are principal/secondary because the other contact may not have the first one as a contact

        public string PrincipalId { get; set; }

        public string SecondaryId { get; set; }

        public string Message { get; set; }

        [ForeignKey("PrincipalId")]
        public virtual User PrincipalUser { get; set; }

        [ForeignKey("SecondaryId")]
        public virtual User SecondaryUser { get; set; }
    }
}
