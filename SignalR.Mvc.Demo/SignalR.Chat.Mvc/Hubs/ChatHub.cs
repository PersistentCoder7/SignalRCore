using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalR.Chat.Mvc.Data;
using SignalR.Chat.Mvc.Models;

namespace SignalR.Chat.Mvc.Hubs
{
    public class ChatHub: Hub
    {
        private  UserManager<ChatUser> _userManager{ get; set; }
        private  ApplicationDbContext _context { get; set; }


        public ChatHub(ApplicationDbContext context, UserManager<ChatUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task BroadcastFromClient(string message)
        {
            try
            {
                var CurrentUser = await _userManager.GetUserAsync(Context.User);

                Message m = new Message()
                {
                    Body = message,
                    TimeStamp = DateTime.Now,
                    FromUser = CurrentUser,

                };
                _context.Messages.Add(m);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("Broadcast",new { 
                    body = m.Body, 
                    fromUser = m.FromUser.Email,
                    timestamp = m.TimeStamp.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture)

                });
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("HubError", new {error= ex.Message});
                throw;
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected",
                new
                {
                    connectedId = Context.ConnectionId,
                    timeStamp = DateTime.Now,
                    messageTimeStamp = DateTime.Now.ToString("hh:mm tt MMM dd", CultureInfo.InvariantCulture)
                }
                );
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", $"User disconnected, ConnectionId: {Context.ConnectionId}");
        }
    }
}
