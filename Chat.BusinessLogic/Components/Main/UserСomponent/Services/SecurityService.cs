using Chat.BusinessLogic.Base.Service;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.UserСomponent.Entities;
using Chat.BusinessLogic.Components.Main.UserСomponent.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.BusinessLogic.Components.Main.UserСomponent.Services
{
	public class SecurityService : BaseService, ISecurityService
	{
		public SecurityService(IConnectionMultiplexer redis) : base(redis)
		{
		}

		public async Task<User> Login(UserLoginDto userLoginDto)
		{
			var usernameKey = $"username:{userLoginDto.Username}";
			var userExists = await _database.KeyExistsAsync(usernameKey);
			if (userExists)
			{
				var userKey = (await _database.StringGetAsync(usernameKey)).ToString();
				var userId = int.Parse(userKey.Split(':').Last());
				return new User()
				{
					Username = userLoginDto.Username,
					Id = userId,
					Online = true
				};
			}
			else
			{
				return null;
			}
		}

		public async Task Logout(HttpContext httpContext)
		{
			httpContext.Session.Remove("user");
			await httpContext.Session.CommitAsync();
		}
	}
}