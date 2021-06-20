using AutoMapper;
using Chat.BusinessLogic.Components.Main.ChatСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.ChatСomponent.Entities;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.UserСomponent.Entities;

namespace Chat.Configs
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<User, UserDto>();
			CreateMap<ChatRoom, ChatRoomDto>();
			CreateMap<ChatRoomMessage, ChatRoomMessageDto>();
		}
	}
}