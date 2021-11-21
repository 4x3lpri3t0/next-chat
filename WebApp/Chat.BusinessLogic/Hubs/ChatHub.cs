using System.Threading.Tasks;
using Chat.BusinessLogic.Components.Main.ChatСomponent.Entities;
using Chat.BusinessLogic.Components.Main.ChatСomponent.Services.Interfaces;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.UserСomponent.Services.Interfaces;
using Newtonsoft.Json;

namespace Chat.BusinessLogic.Hubs
{
    public class ChatHub : BaseHub
    {
        private readonly IChatService chatService;

        public ChatHub(IUserService userService, IChatService chatService) : base(userService)
        {
            this.chatService = chatService;
        }

        public async Task SendMessage(string userString, string messageString)
        {
            var message = JsonConvert.DeserializeObject<ChatRoomMessage>(messageString);

            var user = JsonConvert.DeserializeObject<UserDto>(userString);

            await chatService.SendMessage(user, message);
        }
    }
}