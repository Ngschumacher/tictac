using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TypeFaster
{
    public class ChatHub : Hub
    {
        
        private readonly static ConnectionMapping<string> _connections = 
            new ConnectionMapping<string>();

        private  static  int _userCount = 0;
        
        public ChatHub()
        {
            
        }
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message + _userCount);
        }
        
      
        public async Task sendToAll(string user, string message)
        {
            _connections.Add(user, Context.ConnectionId);
            
            await Clients.All.SendAsync("sendToAll", user, message + _userCount);
            
            await UserChange();
        }
        
        public async Task SignIn(string user)
        {
            _connections.Add(user, Context.ConnectionId);
            
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

            _connections.Remove(name, Context.ConnectionId);
            
            return base.OnDisconnectedAsync(exception);
        }

        public async Task UserChange()
        {
            var userNames = _connections.GetKeys();
            
            await Clients.All.SendAsync("connections", userNames);
        }
        
    }
}