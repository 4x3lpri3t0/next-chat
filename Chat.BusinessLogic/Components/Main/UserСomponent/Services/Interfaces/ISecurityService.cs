using System.Threading.Tasks;
using Chat.BusinessLogic.Base.Service.Interfaces;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.UserСomponent.Entities;
using Microsoft.AspNetCore.Http;

namespace Chat.BusinessLogic.Components.Main.UserСomponent.Services.Interfaces
{
    public interface ISecurityService : IBaseService
    {
        Task<User> Login(UserLoginDto userLoginDto);

        Task Logout(HttpContext httpContext);
    }
}