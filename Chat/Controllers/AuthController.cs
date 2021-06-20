﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Chat.BusinessLogic.Components.Main.UserСomponent.Services.Interfaces;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;
using Chat.Controllers.Base;
using Chat.BusinessLogic.Helpers;

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

			var user = await securityService.Login(userLoginDto);
			if (user == null)
			{
				return new NotFoundResult();
			}

			var res = user.ToDto<UserDto>();

			await HttpContext.Session.LoadAsync();

			var userString = JsonSerializer.Serialize(res);
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