using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChat.Context
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string PrincipalId { get; set; }

        public string SecondaryId { get; set; }

        //The string could be the message, the image link or the audio link (another method perhabs)
        public string String { get; set; }

        //0 = message // 1 = video // 2 = image
        public int Type { get; set; }

        public DateTime Datetime { get; set; }
    }
}
