using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Mvc.Demo.Hubs
{
    public class WeatherHub: Hub
    {
        public async Task BroadcastFromClient(string message)
        {
            Clients.All.SendAsync("Broadcast", message);
        }
    }
}
