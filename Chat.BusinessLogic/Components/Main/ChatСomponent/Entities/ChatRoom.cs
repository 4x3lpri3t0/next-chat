using System.Collections.Generic;
using Chat.DataAccess.Entities;

namespace Chat.BusinessLogic.Components.Main.ChatСomponent.Entities
{
    public class ChatRoom : BaseEntity
    {
        public string Id { get; set; }
        public IEnumerable<string> Names { get; set; }
    }
}