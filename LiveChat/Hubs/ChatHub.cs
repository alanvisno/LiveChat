using LiveChat.Context;
using LiveChat.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChat.Hubs
{
    public class ChatHub : Hub
    {

        //SINGLETON PATTERN (to use the same instance for each call)
        private readonly static ChatHub _instance = new ChatHub();

        private ChatHub()
        {

        }

        public static ChatHub Instance
        {
            get
            {
                return _instance;
            }
        }
        ///////////////////////////////////

        public async Task SendMessage(MessageModel model) =>
            await Clients.All.SendAsync("Receive", model.userid, model.message);
    }
}
