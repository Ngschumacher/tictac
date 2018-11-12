using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicTac.Core.Interfaces;
using TicTac.Website.Hubs.Models;

namespace TicTac.Website.Hubs {
    public static class Users {
        public static readonly ConnectionMapping<int> Connections =
            new ConnectionMapping<int>();
    }

    public class ChatHub : Hub {
        private readonly IGameService _gameService;
        private readonly IUserService _userService;

        public ChatHub(IGameService gameService, IUserService userService) {
            _gameService = gameService;
            _userService = userService;
        }

        public async Task SignIn(string username) {
            var user = _userService.GetOrCreateUser(username);
            Users.Connections.Add(user.Id, Context.ConnectionId);

            await Clients.Client(Context.ConnectionId).SendAsync("userInformation", user);
            await UserChange();
        }


        public override Task OnConnectedAsync() {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception) {
            var user = Users.Connections.GetAll().Find(x => x.Value.Contains(Context.ConnectionId));
            Users.Connections.Remove(user.Key, Context.ConnectionId);

            UserChange().Wait();

            return base.OnDisconnectedAsync(exception);
        }

        private async Task UserChange() {
            var userIds = Users.Connections.GetKeys();
            var users = _userService.GetUsers(userIds);

            await Clients.All.SendAsync("connections", users);
        }
    }
}