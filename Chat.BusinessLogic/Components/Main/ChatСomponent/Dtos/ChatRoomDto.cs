using Chat.BusinessLogic.Dtos;
using System.Collections.Generic;

namespace Chat.BusinessLogic.Components.Main.ChatСomponent.Dtos
{
	public class ChatRoomDto : BaseDto
	{
		public string Id { get; set; }
		public IEnumerable<string> Names { get; set; }
	}
}