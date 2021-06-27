using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SignalR.Chat.Mvc.Models
{
    public class Message
    {
        [Key]
        public int Id {get; set;}
        public string Body {get; set;}
        public DateTime TimeStamp { get; set; }
        public virtual ChatUser FromUser { get; set; }
    }

    public class ChatUser: IdentityUser
    {
        public virtual  ICollection<Message> Messages { get; set; }
    }
}
