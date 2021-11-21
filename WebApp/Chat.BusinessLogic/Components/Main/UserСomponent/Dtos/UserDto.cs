using Chat.BusinessLogic.Dtos;

namespace Chat.BusinessLogic.Components.Main.UserСomponent.Dtos
{
    public class UserDto : BaseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool Online { get; set; } = false;
    }
}