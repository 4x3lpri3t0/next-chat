using System.Linq;
using System.Threading.Tasks;
using Chat.BusinessLogic.Components.Main.ChatСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.ChatСomponent.Services.Interfaces;
using Chat.BusinessLogic.Helpers;
using Chat.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    /// <summary>
    /// Used to retrieve the room-related data.
    /// </summary>
    public class RoomsController : ApiController
    {
        private readonly IChatService chatService;

        public RoomsController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        /// <summary>
        /// Create a new room.
        /// </summary>
        [HttpPut("{roomName}")]
        public async Task<IActionResult> CreateRoom(string roomName = "NewRoom")
        {
            var room = await chatService.CreateChatRoom(roomName);

            return Ok(room.ToDto<ChatRoomDto>());
        }

        /// <summary>
        /// Get rooms for specific user id.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRoom(int userId = 0)
        {
            var rooms = await chatService.GetRooms(userId);

            return Ok(rooms.Select(x => x.ToDto<ChatRoomDto>()));
        }

        /// <summary>
        /// Get messages for a given room.
        /// </summary>
        [HttpGet("messages/{roomId}")]
        public async Task<IActionResult> GetMessages(string roomId = "0", int offset = 0, int size = 50)
        {
            var messages = await chatService.GetMessages(roomId, offset, size);

            return Ok(messages.Select(x => x.ToDto<ChatRoomMessageDto>()));
        }
    }
}