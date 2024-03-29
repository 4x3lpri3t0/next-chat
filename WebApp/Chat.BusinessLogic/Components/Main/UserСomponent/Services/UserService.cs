﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.BusinessLogic.Base.Service;
using Chat.BusinessLogic.Components.Main.UserСomponent.Dtos;
using Chat.BusinessLogic.Components.Main.UserСomponent.Services.Interfaces;
using StackExchange.Redis;

namespace Chat.BusinessLogic.Components.Main.UserСomponent.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IConnectionMultiplexer redis) : base(redis)
        {
        }

        public async Task<IDictionary<string, UserDto>> Get(int[] ids)
        {
            var users = new Dictionary<string, UserDto>();
            foreach (var id in ids)
            {
                users.Add(id.ToString(), new UserDto()
                {
                    Id = id,
                    Username = await _database.HashGetAsync($"user:{id}", "username"),
                    Online = await _database.SetContainsAsync("online_users", id.ToString())
                });
            }

            return users;
        }

        public async Task<IDictionary<string, UserDto>> GetOnline()
        {
            var onlineIds = await _database.SetMembersAsync("online_users");
            var users = new Dictionary<string, UserDto>();
            foreach (var onlineIdRedisValue in onlineIds)
            {
                var onlineId = onlineIdRedisValue.ToString();
                var user = await _database.HashGetAsync($"user:{onlineId}", "username");
                users.Add(onlineId, new UserDto()
                {
                    Id = int.Parse(onlineId),
                    Username = user.ToString(),
                    Online = true
                });
            }

            return users;
        }

        public async Task OnStartSession(UserDto user)
        {
            await _database.SetAddAsync("online_users", user.Id);
            user.Online = true;
            await PublishMessage("user.connected", user.Username);
        }

        public async Task OnStopSession(UserDto user)
        {
            await _database.SetRemoveAsync("online_users", user.Id);
            user.Online = false;
            await PublishMessage("user.disconnected", user.Username);
        }
    }
}