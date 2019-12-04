using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM
{
    public class ChatHub:Hub
    {
        
        public Task SendMsg(string username,string message)
        {
            return Clients.All.SendAsync("Show", username , message);
        }
        //public Task SendMsgToUser(int userid,string message)
        //{
        //    return Clients.Client(userid).SendAsync();
        //}

        
    }
}
