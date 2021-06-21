using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.BusinessLogic.Base.Service.Interfaces;
using Chat.BusinessLogic.Components.Main.ChatСomponent.Entities;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;

namespace Chat.BusinessLogic.Components.Main.ChatСomponent.Services.Interfaces
{
    public interface IChatService : IBaseService
    {
        Task<ChatRoom> CreateChatRoom(string roomName);

        Task AddUserToChatRoom(string roomId, string userId);

        Task<List<ChatRoom>> GetRooms(int userId = 0);

        Task<List<ChatRoomMessage>> GetMessages(string roomId = "0", int offset = 0, int size = 50);

        Task SendMessage(UserDto user, ChatRoomMessage message);
    }
}