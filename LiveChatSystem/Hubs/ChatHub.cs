using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Configuration;

namespace LiveChatSystem.Hubs
{
   
    public class ChatHub : Hub
    {
        String strConnString = ConfigurationManager.ConnectionStrings["DbLiveChat"].ConnectionString;
        public void Hello()
        {
            Clients.All.hello();
        }

        [HubMethodName("sendMessages")]
        public static void SendMessages(string userid, string clientId, string recepientPhoneNumber, string recipientId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.updateMessages(userid, clientId, recepientPhoneNumber, recipientId);
            
        }

        public void Send(string name, string message, string connId)
        {
            Clients.Client(connId).addNewMessageToPage(name, message);
        }
        
        
        ////Code type 1 to send to all
        //public void Send(string name, string message)
        //{

        //    // Call the addNewMessageToPage method to update clients.
        //    //  Clients.All.addNewMessageToPage(name, message);
        //    Clients.User(name).send(message);

        //}
        ////public override Task OnConnected()
        ////{
        ////    string name = Context.User.Identity.Name;
        ////    Groups.Add(Context.ConnectionId, name);

        ////    return base.OnConnected();
        ////}

        ////New code 2. send specific
        //private readonly static ConnectionMapping<string> _connections =
        //   new ConnectionMapping<string>();

        //public void SendChatMessage(string who, string message)
        //{
        //    string name = Context.User.Identity.Name;

        //    foreach (var connectionId in _connections.GetConnections(who))
        //    {
        //        Clients.Client(connectionId).addChatMessage(name + ": " + message);
        //    }
        //}

        //public override Task OnConnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Add(name, Context.ConnectionId);

        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Remove(name, Context.ConnectionId);

        //    return base.OnDisconnected(stopCalled);
        //}

        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}