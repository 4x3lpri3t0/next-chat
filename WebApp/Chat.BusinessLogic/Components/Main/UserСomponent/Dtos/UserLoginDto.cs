using System.ComponentModel.DataAnnotations;

namespace Chat.BusinessLogic.Components.Main.UserСomponent.Dtos
{
    public class UserLoginDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}