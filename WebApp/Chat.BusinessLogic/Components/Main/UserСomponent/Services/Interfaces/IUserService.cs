using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.BusinessLogic.Base.Service.Interfaces;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;

namespace Chat.BusinessLogic.Components.Main.UserСomponent.Services.Interfaces
{
    public interface IUserService : IBaseService
    {
        Task<IDictionary<string, UserDto>> Get(int[] ids);

        Task<IDictionary<string, UserDto>> GetOnline();

        Task OnStartSession(UserDto user);

        Task OnStopSession(UserDto user);
    }
}