using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChat.Model
{
    public class ContactDataModel
    {
        public string PrincipalId { get; set; }
        public string PrincipalName { get; set; }

        public string SecondaryId { get; set; }
        public string SecondaryName { get; set; }

        public string Message { get; set; }
    }
}
