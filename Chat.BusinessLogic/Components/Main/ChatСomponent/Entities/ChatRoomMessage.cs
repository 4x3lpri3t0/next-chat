using Chat.DataAccess.Entities;

namespace Chat.BusinessLogic.Components.Main.ChatСomponent.Entities
{
    public class ChatRoomMessage : BaseEntity
    {
        public string From { get; set; }
        public int Date { get; set; }
        public string Message { get; set; }
        public string RoomId { get; set; }
    }
}