using Chat.DataAccess.Entities;

namespace Chat.BusinessLogic.Components.Main.UserСomponent.Entities
{
	public class User : BaseEntity
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public bool Online { get; set; } = false;
	}
}
