using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TypeFaster.Core.Interfaces;

namespace TypeFaster
{
    public static class Users
    {
        public static readonly ConnectionMapping<int> Connections = 
            new ConnectionMapping<int>();
    }
    
    public class ChatHub : Hub
    {
        private readonly IBoardService _boardService;
        private readonly IUserService _userService;

        

        private  static  int _userCount = 0;
        
        public ChatHub(IBoardService boardService, IUserService userService)
        {
            _boardService = boardService;
            _userService = userService;
        }

        public async Task sendChallenge(int challengerId, int challengedRecieverId)
        {

            var connections = Users.Connections.GetConnections(challengedRecieverId).ToList();

            var game = _boardService.NewGame(challengerId, challengedRecieverId);

            var challenge = new ChallengeViewModel()
            {
                ChallengerName = game.Player2.Username,
                GameId = game.Id
            };
            
            await Clients.Clients(connections).SendAsync("challengeRecieved", challenge);
        }
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message + _userCount);
        }
      
        public async Task sendToAll(string user, string message)
        {
            
            await Clients.All.SendAsync("sendToAll", user, message + _userCount);
            
            await UserChange();
        }
        
        public async Task SignIn(string username)
        {

            
            var user = _userService.GetOrCreateUser(username);
            
            Users.Connections.Add(user.Id, Context.ConnectionId);

            await Clients.Client(Context.ConnectionId).SendAsync("userInformation", user);
            
            await UserChange();
        }


        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            string name = Context.User.Identity.Name;
            var user = _userService.GetOrCreateUser(name);
            Users.Connections.Remove(user.Id, Context.ConnectionId);
            
            return base.OnDisconnectedAsync(exception);
        }

        public async Task UserChange()
        {
            var userIds = Users.Connections.GetKeys();

            var users = _userService.GetUsers(userIds);
            
            await Clients.All.SendAsync("connections", users);
        }
        
    }

    public class ChallengeViewModel
    {
        public int GameId { get; set; }
        public string ChallengerName { get; set; }
    }
}