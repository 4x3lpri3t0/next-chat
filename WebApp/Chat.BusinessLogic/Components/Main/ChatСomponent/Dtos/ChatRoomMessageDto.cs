using Chat.BusinessLogic.Dtos;

namespace Chat.BusinessLogic.Components.Main.ChatСomponent.Dtos
{
    public class ChatRoomMessageDto : BaseDto
    {
        public string From { get; set; }
        public int Date { get; set; }
        public string Message { get; set; }
        public string RoomId { get; set; }
    }
}