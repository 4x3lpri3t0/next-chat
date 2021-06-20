using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.UserСomponent.Services.Interfaces;
using Chat.BusinessLogic.Helpers;
using Chat.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    public class AuthController : ApiController
    {
        private readonly ISecurityService securityService;

        public AuthController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        public class LoginData
        {
            [Required]
            public string username { get; set; }

            [Required]
            public string password { get; set; }
        }

        /// <summary>
        /// Create user session by username and password.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            BusinessLogic.Components.Main.UserСomponent.Entities.User user = await securityService.Login(userLoginDto);
            if (user == null)
            {
                return new NotFoundResult();
            }

            UserDto res = user.ToDto<UserDto>();

            await HttpContext.Session.LoadAsync();

            string userString = JsonSerializer.Serialize(res);
            HttpContext.Session.SetString("user", userString);
            await HttpContext.Session.CommitAsync();

            return Ok(res);
        }

        /// <summary>
        /// Dispose the user session.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await securityService.Logout(HttpContext);
            return Ok();
        }
    }
}